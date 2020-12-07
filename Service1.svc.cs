using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {
        public List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Customer", connection);
                connection.Open();
                SqlDataReader rows = command.ExecuteReader();
                while (rows.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = (int)rows["Id"],
                        Firstname = (string)rows["Firstname"],
                        Lastname = (string)rows["Lastname"],
                        Birthday = (DateTime)rows["Birthday"]
                    };
                    customers.Add(customer);
                }
            }
            return customers;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<Order> GetOrders(int id)
        {
            var orders = new List<Order>();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE IdCustomer=@id", connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                connection.Open();
                SqlDataReader rows = command.ExecuteReader();
                while (rows.Read())
                {
                    Order order = new Order
                    {
                        Id = (int)rows["Id"],
                        Title = (string)rows["Title"],
                        IdCustomer = (int)rows["IdCustomer"],
                        Price = (int)rows["Price"],
                        Count = (int)rows["Count"]
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }
    }
}
