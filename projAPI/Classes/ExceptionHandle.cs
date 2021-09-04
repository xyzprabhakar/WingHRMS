using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
    public class ExceptionHandle
    {
        public bool EF_ErrorHandler(Exception exception)
        {
            if (exception is DbUpdateConcurrencyException concurrencyEx)
            {
                // A custom exception of yours for concurrency issues
                throw new Exception("Concurrency Error !!!");
            }
            else if (exception is DbUpdateException dbUpdateEx)
            {
                if (dbUpdateEx.InnerException != null
                     && dbUpdateEx.InnerException.InnerException != null)
                {
                    if (dbUpdateEx.InnerException.InnerException is SqlException sqlException)
                    {
                        switch (sqlException.Number)
                        {
                            case 2627:  // Unique constraint error
                            case 547:   // Constraint check violation
                            case 2601:  // Duplicated key row error
                                        // Constraint violation exception

                                return true;
                        }

                    }
                    else if (dbUpdateEx.InnerException.InnerException is MySqlException mysqlException)
                    {
                        switch (mysqlException.Number)
                        {
                            case 1062:
                                throw new Exception("Duplicate data");
                            case 1452:
                                var data = mysqlException.Message.Split("FOREIGN KEY (")[1];
                                throw new Exception("Invalid Data " + data.Split(")")[0]);

                        }
                        throw new Exception(exception.Message);
                    }
                }
                else if (dbUpdateEx.InnerException is MySqlException mysqlException)
                {
                    switch (mysqlException.Number)
                    {
                        case 1062:
                            throw new Exception("Duplicate data");
                        case 1452:
                            var data = mysqlException.Message.Split("FOREIGN KEY (")[1];
                            throw new Exception("Invalid Data " + data.Split(")")[0]);

                    }
                }
                throw new Exception(exception.Message);

            }
            else
            {
                throw new Exception(exception.Message);
            }
            
        }
    }
}
