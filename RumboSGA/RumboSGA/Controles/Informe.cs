using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions;
using CrystalDecisions.Windows.Forms;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using CrystalDecisions.Shared;

namespace RumboSGA.Controles
{
    public partial class Informe : ReportDocument
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReportDocument ReportLoad(string nameReport)
        {
            CrystalReportViewer rv = new CrystalReportViewer();
            ReportDocument reportDocument = new ReportDocument();
            try
            {
                reportDocument.Load(nameReport);

                //CrystalReportViewer.ReportSource = reportDocument;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            return reportDocument;
        }

        public static ReportDocument AplicarNuevoServidor(ReportDocument report, string serverName, string username, string password, string databaseName)
        {
            log.Debug("Cargando informe credenciales " + serverName + " " + username + " " + password + " " + databaseName);
            //report.Refresh();
            // report.SetDatabaseLogon(username, password, serverName, databaseName);

            foreach (ReportDocument subReport in report.Subreports)
            {
                foreach (Table crTable in subReport.Database.Tables)
                {
                    TableLogOnInfo loi = crTable.LogOnInfo;

                    loi.ConnectionInfo.ServerName = serverName;
                    loi.ConnectionInfo.UserID = username;
                    loi.ConnectionInfo.Password = password;
                    loi.ConnectionInfo.DatabaseName = databaseName;
                    loi.ConnectionInfo.IntegratedSecurity = false;
                    crTable.LogOnInfo.ConnectionInfo = loi.ConnectionInfo;
                    crTable.ApplyLogOnInfo(loi);
                }
            }

            foreach (Table crTable in report.Database.Tables)
            {
                TableLogOnInfo loi = (CrystalDecisions.Shared.TableLogOnInfo)crTable.LogOnInfo.Clone();
                log.Debug(crTable.LogOnInfo.ConnectionInfo.ServerName);
                log.Debug(loi.ConnectionInfo.ServerName);
                //loi.TableName = crTable.Name;
                loi.ConnectionInfo.ServerName = serverName;
                loi.ConnectionInfo.UserID = username;
                loi.ConnectionInfo.Password = password;
                loi.ConnectionInfo.DatabaseName = databaseName;
                loi.ConnectionInfo.IntegratedSecurity = false;
                loi.ConnectionInfo.AllowCustomConnection = true;
                //crTable.LogOnInfo.ConnectionInfo = loi.ConnectionInfo;
                TableLogOnInfo myTableLoi = crTable.LogOnInfo;
                myTableLoi.ConnectionInfo = loi.ConnectionInfo;
                crTable.ApplyLogOnInfo(myTableLoi);
                log.Debug("Servidor despues " + crTable.LogOnInfo.ConnectionInfo.ServerName);
            }

            return report;
        }

        internal static string GetPathInforme(int idInforme)
        {
            try
            {
                String ruta = Business.GetRutaInforme(idInforme);
                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha encontrado el informe número" + 14);
            }
        }

        public static ReportDocument AplicarNuevoServidor(ReportDocument report, string serverName, string databaseName, bool integratedSecurity)
        {
            foreach (ReportDocument subReport in report.Subreports)
            {
                foreach (Table crTable in subReport.Database.Tables)
                {
                    TableLogOnInfo loi = crTable.LogOnInfo;
                    loi.ConnectionInfo.ServerName = serverName;
                    loi.ConnectionInfo.DatabaseName = databaseName;

                    if (integratedSecurity)
                    {
                        loi.ConnectionInfo.IntegratedSecurity = true;
                    }

                    crTable.LogOnInfo.ConnectionInfo = loi.ConnectionInfo;
                    crTable.ApplyLogOnInfo(loi);
                }
            }

            foreach (Table crTable in report.Database.Tables)
            {
                TableLogOnInfo loi = crTable.LogOnInfo;
                loi.ConnectionInfo.ServerName = serverName;
                loi.ConnectionInfo.DatabaseName = databaseName;
                if (integratedSecurity)
                {
                    loi.ConnectionInfo.IntegratedSecurity = true;
                }
                crTable.LogOnInfo.ConnectionInfo = loi.ConnectionInfo;
                crTable.ApplyLogOnInfo(loi);
            }
            return report;
        }

        public static ReportDocument AplicarNuevoDatabaseName(ReportDocument report, string databaseName, String schemaName)
        {
            string prefix = "";

            if (!schemaName.Equals(""))
            {
                prefix = String.Format("{0}.{1}.", databaseName, schemaName);
            }
            else
            {
                prefix = String.Format("{0}.", databaseName);
            }

            foreach (Table crTable in report.Database.Tables)
            {
                crTable.Location = String.Format("{0}{1}", prefix, crTable.Location.Substring(crTable.Location.LastIndexOf(".") + 1));
            }
            return report;
        }

        public static ReportDocument AplicarCredentialsDesdeConnectionString(ReportDocument report, string sqlConnectionString)
        {
            SqlConnectionStringBuilder cb = new System.Data.SqlClient.SqlConnectionStringBuilder(sqlConnectionString);
            log.Debug("AplicarCredentialsDesdeConnectionString " + sqlConnectionString);
            if (!cb.IntegratedSecurity)
            {
                log.Debug("Sin seguridad integrada " + cb.DataSource + " " + cb.UserID + " " + cb.Password + " " + cb.InitialCatalog);
                return AplicarNuevoServidor(report, cb.DataSource, cb.UserID, cb.Password, cb.InitialCatalog);
            }
            else
            {
                log.Debug("Con seguridad integrada " + cb.DataSource + " " + cb.UserID + " " + cb.Password + " " + cb.InitialCatalog);
                return AplicarNuevoServidor(report, cb.DataSource, cb.InitialCatalog, true);
            }
        }

        public static bool ExisteParametro(ReportDocument report, ref string paramName)
        {
            if (report == null || report.ParameterFields == null)
            {
                return false;
            }

            foreach (ParameterField param in report.ParameterFields)
            {
                if (paramName.Equals(param.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static ParameterFieldDefinition GetParametroPorNombre(ReportDocument report, ref string paramName)
        {
            if (report == null || report.ParameterFields == null)
            {
                return null;
            }

            foreach (ParameterFieldDefinition param in report.DataDefinition.ParameterFields)
            {
                if (paramName.Equals(param.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return param;
                }
            }

            return null;
        }

        public static ReportDocument AplicarParametros(ReportDocument report, string parameters)
        {
            return AplicarParametros(report, parameters, false);
        }

        public static ReportDocument AplicarParametros(ReportDocument report, string parameters, bool removeInvalidParameters)
        {
            if (string.IsNullOrEmpty(parameters) || !parameters.Contains("="))
            {
                return report;
            }
            parameters = parameters.Trim(';');

            string[] parameterList = parameters.Split(';');

            foreach (string parameter in parameterList)
            {
                String[] nameValue = parameter.Split('=');
                if (!ExisteParametro(report, ref nameValue[0]) && !removeInvalidParameters)
                {
                    throw new Exception(String.Format("The parameter '{0}' does not exist in the Crystal Report.", nameValue[0]));
                }
                else if (!ExisteParametro(report, ref nameValue[0]) && removeInvalidParameters)
                {
                    continue;
                }
                ParameterFieldDefinition pfd = GetParametroPorNombre(report, ref nameValue[0]);
                //report.DataDefinition.ParameterFields(nameValue[0]);

                ParameterValues pValues;
                ParameterDiscreteValue parm;
                pValues = new ParameterValues();

                parm = new ParameterDiscreteValue();
                parm.Value = nameValue[1];
                pValues.Add(parm);
                pfd.ApplyCurrentValues(pValues);
            }
            return report;
        }
    }
}