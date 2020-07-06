﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repositorio.Dal
{ 
    public class Utils
    {

   

        public static void BulkInsert<T>(string connection, string tableName, IList<T> list)
        { 

            using (var bulkCopy = new SqlBulkCopy(connection))
            {

                var table = new DataTable();
                try
                {
                    bulkCopy.BulkCopyTimeout = 0;

                    bulkCopy.BatchSize = list.Count;

                    bulkCopy.DestinationTableName = tableName;

                    var props = TypeDescriptor.GetProperties(typeof(T))
                                               //Dirty hack to make sure we only have system data types 
                                               //i.e. filter out the relationships/collections
                                               .Cast<PropertyDescriptor>()
                                               .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                               .ToArray();

                    foreach (var propertyInfo in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                    }

                    var values = new object[props.Length];
                    foreach (var item in list)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }

                        table.Rows.Add(values);
                    }

                    bulkCopy.WriteToServer(table);
                }
                catch (Exception e)
                {

                    throw;
                }
                finally { table.Clear(); }



            }
        }

    }
}
