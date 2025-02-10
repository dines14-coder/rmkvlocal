using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Demo.DataModel;
using static Demo.DataModel.FileConfig;
using RmKV.NXT.SilkWARP.DataModel.Model.Shared;

namespace Demo.DataModel.utilities
{
    public static class Utilities
    {
        public static string SerializeObject(Object obj)
        {
            return SerializeObject(obj, false);
        }


        public static string SerializeObject(Object obj, bool generateNamespace)
        {
            XmlSerializerNamespaces ns = null;
            XmlSerializer ser = new XmlSerializer(obj.GetType()); ;

            if (generateNamespace)
            {
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
            }
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                if (ns == null)
                    ser.Serialize(writer, obj);
                else
                    ser.Serialize(writer, obj, ns);
                writer.Flush();
                writer.Close();

            }
            return sb.ToString();
        }


        //public static string JsonSerializeObject(object obj)
        //{
        //    JavaScriptSerializer objJsonSer = new JavaScriptSerializer();
        //    return objJsonSer.Serialize(obj);

        //}

        public static T deserializeObject<T>(string serializedObject)
        {
            TextReader objTextReaader = new StringReader(serializedObject);
            XmlSerializer objXmlSer = new XmlSerializer(typeof(T));
            return ((T)(Convert.ChangeType(objXmlSer.Deserialize(objTextReaader), typeof(T))));

        }

        //public static T JsonDeserializeObject<T>(string serializedObject)
        //{
        //    JavaScriptSerializer objJsonSer = new JavaScriptSerializer();
        //    return ((T)(Convert.ChangeType(objJsonSer.Deserialize<T>(serializedObject), typeof(T))));
        //}

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ToDataTable<T>(this IList<T> objList)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable objDataTable = new DataTable();
            foreach (PropertyDescriptor property in properties)
                objDataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            if (objList != null)
            {
                foreach (T item in objList)
                {
                    DataRow row = objDataTable.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    objDataTable.Rows.Add(row);
                }
            }
            return objDataTable;
        }

        public static DataTable ConvertEntityListToDataTable<T>(this IList<T> objList, List<Type> excludedTypes)
        {
            if (excludedTypes == null)
                excludedTypes = new List<Type>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable objDataTable = new DataTable();
            foreach (PropertyDescriptor property in properties)
                if (!excludedTypes.Contains(property.PropertyType))
                    objDataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            if (objList != null)
            {
                foreach (T item in objList)
                {
                    DataRow row = objDataTable.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    objDataTable.Rows.Add(row);

                }
            }
            return objDataTable;
        }

        private static T ConvertDataRowToEntity<T>(this DataRow tableRow) where T : new()
        {
            //create a new type of the entity i want 
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                //Look for the object's property with the columns name , ignore case
                PropertyInfo pInfo = t.GetProperty(colName.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                //did we find the property ?

                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    //is this Nullable<> type 

                    bool ISNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (ISNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;

                        }

                        else
                        {
                            //cont thee db type into the T we have in our Nullable<T> type
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));

                        }
                    }
                    else
                    {
                        // convert the db type into the type of the proerty in our entity 
                        if (pInfo.PropertyType.IsEnum)
                            val = Enum.ToObject(pInfo.PropertyType, val);
                        else
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    //set the value of the property with the value from the db
                    pInfo.SetValue(returnObject, val, null);

                }

            }
            //return the entity object with values
            return returnObject;
        }

        public static List<T> ConvertDataTableToEntityList<T>(this DataTable table) where T : new()
        {
            Type t = typeof(T);

            //create a list of the entities we wnat to return 
            List<T> returnObject = new List<T>();

            //Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                //convert each row into an entity object and add to the list
                T newRow = dr.ConvertDataRowToEntity<T>();
                returnObject.Add(newRow);
            }
            return returnObject;
        }

        public static string Base64Encode(string plainText)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }


        public static string Base64Decode(string base64EncodedData)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64EncodedData));
        }

        public static string SerializeObjectUsingDataContractSerializer(Object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var sw = new StringWriter())
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = System.Xml.Formatting.None;
                    serializer.WriteObject(writer, obj);
                    writer.Flush();
                    return sw.ToString();
                }
            }
        }
        public class XMLFromObject
        {
            public string GetXMLFromObject(object o)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter tw = null;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(o.GetType());
                    tw = new XmlTextWriter(sw);
                    serializer.Serialize(tw, o);
                }
                catch (Exception ex)
                {
                    //Handle Exception Code
                    throw ex;
                }
                finally
                {
                    sw.Close();
                    if (tw != null)
                    {
                        tw.Close();
                    }
                }
                return sw.ToString();
            }
        }

        public static FileStreamResult ReturnStreamReport(DataSet dataSet, string webRootPath)
        {
            MemoryStream stream = new MemoryStream();
            var contentType = "application/pdf";

            if (dataSet.Tables[0].Rows.Count != 0)
            {
                using (Report rpt = new Report())
                {
                    rpt.Load(webRootPath);
                    rpt.RegisterData(dataSet, "NorthWind");
                    rpt.PreparePhase1();
                    rpt.PreparePhase2();
                    PDFSimpleExport pdf = new PDFSimpleExport();
                    rpt.Export(pdf, stream);
                }
                stream.Flush();
                byte[] file = stream.ToArray();
                MemoryStream ms1 = new MemoryStream();
                ms1.Write(file, 0, file.Length);
                ms1.Position = 0;
                byte[] bytes = ms1.ToArray();
                return new FileStreamResult(ms1, contentType);
            }
            else
            {
                return new FileStreamResult(stream, contentType);
            }
        }

        public static FileConfig GetFileNameAndPath(string fileRootPath, string screenPath, string fileOriginName)
        {
            string extension = Path.GetExtension(fileOriginName);
            var fileName = string.Format(@"{0}" + extension, Guid.NewGuid());
            var filePath = screenPath + "\\" + fileName;
            var fileSavePath = fileRootPath + "\\" + filePath;
            var directory = fileRootPath + "\\" + screenPath + "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileConfig fileConfig = new FileConfig();
            fileConfig.fileName = screenPath + "\\" + fileName;
            fileConfig.fileSavePath = fileSavePath;

            return fileConfig;
        }
        //    public static FileAfterAttachment AttachFiles(IFormFile[] files, List<int> removedIndex, List<OldFiles> oldFiles, string FileAttachPath, List<int> index,string screenName)
        //    {
        //        FileAfterAttachment returnableFile = new FileAfterAttachment();
        //        returnableFile.attachedFiles = new List<Files>();
        //        returnableFile.removedFile = new List<Files>();
        //        if (removedIndex.Count > 0)
        //        {
        //            for (int i = 0; i < removedIndex.Count; i++)
        //            {
        //                for (int j = 0; j < oldFiles.Count; j++)
        //                {
        //                    int dataIndex = Convert.ToInt32(removedIndex[i]);
        //                    if (oldFiles[j] != null)
        //                    {
        //                        if (dataIndex == oldFiles[j].index)
        //                        {
        //                            var directory = FileAttachPath + "\\" + oldFiles[j].name.Split("\\")[0];
        //                            string fileName = oldFiles[j].name.Split("\\")[1];
        //                            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        //                            FileInfo[] OldexistFiles = directoryInfo.GetFiles(fileName.Split(".")[0] + ".*");
        //                            if (OldexistFiles.Length > 0)
        //                            {
        //                                foreach (FileInfo file in OldexistFiles)
        //                                {
        //                                    file.Delete();
        //                                    Files removed = new Files();
        //                                    removed.dataIndex = Convert.ToInt32(dataIndex);
        //                                    removed.file_path = "";
        //                                    returnableFile.removedFile.Add(removed);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (index.Count > 0)
        //        {
        //            if (files != null && files.Length > 0)
        //            {
        //                for (int i = 0; i < index.Count; i++)
        //                {
        //                    if (files[i].FileName != null && FileAttachPath.ToString() != null)
        //                    {
        //                        int dataIndex = Convert.ToInt32(index[i]);
        //                        FileConfig fileConfig = Utilities.GetFileNameAndPath(FileAttachPath, screenName, files[i].FileName);
        //                        var directory = FileAttachPath + "\\" + screenName + "\\";
        //                        if (!Directory.Exists(directory))
        //                        {
        //                            Directory.CreateDirectory(directory);
        //                        }
        //                        using (var stream = new FileStream(fileConfig.fileSavePath, FileMode.Create))
        //                        {
        //                            files[i].CopyTo(stream);
        //                            Files attached = new Files();
        //                            attached.dataIndex = Convert.ToInt32(dataIndex);
        //                            attached.file_path = fileConfig.fileName.ToString();
        //                            returnableFile.attachedFiles.Add(attached);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return returnableFile;
        //    }
        //}

        public static FileConfig GetFileNameAndPath(string fileRootPath, string screenPath, string fileOriginName, string companyCode)
        {
            var cDate = DateTime.Now.ToString();
            string currentDate = Convert.ToDateTime(cDate).Year.ToString() + Convert.ToDateTime(cDate).Month.ToString().PadLeft(2, '0') + Convert.ToDateTime(cDate).Day.ToString().PadLeft(2, '0');
            string extension = Path.GetExtension(fileOriginName);
            var fileName = string.Format(@"{0}" + extension, Guid.NewGuid());
            var filePath = currentDate + "/" + companyCode + "/" + screenPath + "/" + fileName;
            var fileSavePath = fileRootPath + "/" + filePath;
            var directory = fileRootPath + "/" + screenPath + "/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileConfig fileConfig = new FileConfig();
            fileConfig.fileName = currentDate + "/" + companyCode + "/" + screenPath + "/" + fileName;
            fileConfig.fileSavePath = fileSavePath;

            return fileConfig;
        }

        public static FileAfterAttachment AttachFiles(List<IFormFile> files, List<int> removedIndex, List<OldFiles> oldFiles, List<OldFiles> tempFiles, string FileAttachPath, List<int> index, string screenName, string companyCode)
        {
            FileAfterAttachment returnableFile = new FileAfterAttachment();
            returnableFile.attachedFiles = new List<Files>();
            returnableFile.removedFile = new List<Files>();
            if (removedIndex.Count > 0)
            {
                for (int i = 0; i < removedIndex.Count; i++)
                {
                    for (int j = 0; j < oldFiles.Count; j++)
                    {
                        int dataIndex = Convert.ToInt32(removedIndex[i]);
                        if (oldFiles[j] != null)
                        {
                            if (dataIndex == oldFiles[j].index)
                            {
                                string[] filePath = oldFiles[j].name.Split(@"/");
                                var directory = FileAttachPath + "/";
                                if (filePath.Length == 2)
                                    directory = directory + "/" + filePath[0];
                                else if (filePath.Length == 4)
                                    directory = directory + filePath[0] + "/" + filePath[1] + "/" + filePath[2];
                                string fileName = filePath[filePath.Length - 1];
                                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                                FileInfo[] OldexistFiles = directoryInfo.GetFiles(fileName.Split(".")[0] + ".*");
                                if (OldexistFiles.Length > 0)
                                {
                                    foreach (FileInfo file in OldexistFiles)
                                    {
                                        file.Delete();
                                        Files removed = new Files();
                                        removed.dataIndex = Convert.ToInt32(dataIndex);
                                        removed.file_path = "";
                                        returnableFile.removedFile.Add(removed);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (index.Count > 0)
            {
                if (files != null && files.Count > 0)
                {
                    for (int i = 0; i < index.Count; i++)
                    {
                        if (files[i].FileName != null && FileAttachPath.ToString() != null)
                        {
                            int dataIndex = Convert.ToInt32(index[i]);
                            var cDate = DateTime.Now.ToString();
                            string currentDate = Convert.ToDateTime(cDate).Year.ToString() + Convert.ToDateTime(cDate).Month.ToString().PadLeft(2, '0') + Convert.ToDateTime(cDate).Day.ToString().PadLeft(2, '0');
                            FileConfig fileConfig = Utilities.GetFileNameAndPath(FileAttachPath, screenName, files[i].FileName, companyCode);
                            var directory = FileAttachPath + "/" + currentDate + "/" + companyCode.ToString() + "/" + screenName + "/";
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }
                            using (var stream = new FileStream(fileConfig.fileSavePath, FileMode.Create))
                            {
                                files[i].CopyTo(stream);
                                Files attached = new Files();
                                attached.dataIndex = Convert.ToInt32(dataIndex);
                                attached.file_path = fileConfig.fileName.ToString();
                                returnableFile.attachedFiles.Add(attached);
                            }
                        }
                    }
                    for (int i = 0; i < tempFiles.Count; i++)
                    {
                        if (tempFiles[i] != null)
                        {
                            if (tempFiles[i].name != null && tempFiles[i].name != "null")
                            {
                                var directory = FileAttachPath + "/" + tempFiles[i].name.Split("/")[0];
                                string fileName = tempFiles[i].name.Split("/")[1];
                                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                                FileInfo[] OldexistFiles = directoryInfo.GetFiles(fileName.Split(".")[0] + ".*");
                                if (OldexistFiles.Length > 0)
                                {
                                    foreach (FileInfo file in OldexistFiles)
                                    {
                                        file.Delete();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return returnableFile;
        }
    }
}
