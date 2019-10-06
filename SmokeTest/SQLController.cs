using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeTest
{
   public class SQLController
    {

        #region Properties
        private string actionText;

        public string ActionText
        {
            get { return actionText; }
            set { actionText = value; }
        }
        private string outcomeText;

        public string OutcomeText
        {
            get { return outcomeText; }
            set { outcomeText = value; }
        }
        private string commentText;

        public string CommentText
        {
            get { return commentText; }
            set { commentText = value; }
        }
        private int stepNumber;

        public int StepNumber
        {
            get { return stepNumber; }
            set { stepNumber = value; }
        }


        private bool resultOfStep;

        public bool ResultOfStep
        {
            get { return resultOfStep; }
            set { resultOfStep = value; }
        }

        #endregion

        public static SQLController Instance = null;
        
        public SQLController()
        {
            Instance = this;
        }

        public object ExecuteScalarQueryOnCorrespondingTable(string query)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.DBConnectionString))
            {

                try
                {
                    conn.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(string.Format(query)))
                    {
                        result = sqlCommand.ExecuteScalar();

                        if (result == null)
                        {
                            throw new Exception(string.Format("Exception when inserting to table. CommandText: {0}", sqlCommand.CommandText));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Exception when inserting to table. Exception message: {0}", ex.Message));
                }
                finally
                {
                    conn.Close();
                }

                return result;

            }
        }

        public void InsertResultToCorrespondingTable(string tableName)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.DBConnectionString))
            {

                try
                {
                    conn.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(string.Format("insert into dbo.'{0}'(step_no,action_text,outcome_text,passed) VALUES({1},'{2}','{3}',{4})",StepNumber,ActionText,OutcomeText,CommentText,ResultOfStep)))
                    {
                        result = sqlCommand.ExecuteNonQuery();

                        if (result == null)
                        {
                            throw new Exception(string.Format("Exception when inserting to table. CommandText: {0}",sqlCommand.CommandText));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Exception when inserting to table. Exception message: {0}", ex.Message));

                }
                finally
                {
                    conn.Close();
                }          

            }
        }

    }
}
