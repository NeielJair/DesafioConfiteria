using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class GlobalDAL
	{
		protected static string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
		public static SqlConnection SetupConnection()
		{
			return new SqlConnection(CONNECTION_STRING);
		}
	}
}
