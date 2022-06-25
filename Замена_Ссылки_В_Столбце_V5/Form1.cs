using System.Net;

using ClosedXML.Excel;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.VisualBasic;

using NLog;

namespace Замена_Ссылки_В_Столбце_V5
{
    public partial class Form1 : Form
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    button1.Enabled = false;
                    string filePath = openFileDialog1.FileName;

                    var wb = new XLWorkbook(filePath);
                    var ws = wb.Worksheet(1);

                    foreach (var row in ws.Rows())
                    {
                        var colsAt10Pos = row.Cells().Where(cell => cell.Address.ColumnNumber == 10);
                        colsAt10Pos.ToList().ForEach(col =>
                        {
                            if (col != null && col.HasHyperlink)
                            {

                                //var wp = new WebProxy("kwts.vkab.ru:3128");
                                //wp.Credentials = CredentialCache.DefaultCredentials;
                                //HttpClient hc = new HttpClient(new LoggingHandler()
                                //{
                                //    Proxy = wp,
                                //    Credentials = CredentialCache.DefaultCredentials
                                //});



                                HttpClient hc = new HttpClient();
                                HttpRequestMessage r = new HttpRequestMessage();
                                r.RequestUri = new Uri(col.Hyperlink.ExternalAddress.ToString());
                                r.Method = HttpMethod.Get;
                                r.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                                r.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                                r.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                                r.Headers.Add("Accept-Language", "en-US,en;q=0.9");
                                r.Headers.Add("Connection", "keep-alive");

                                HttpResponseMessage response = null;
                                try
                                {
                                    response = hc.SendAsync(r).Result;
                                    //   response.EnsureSuccessStatusCode();
                                    Console.WriteLine();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    //  var   resp = e.Response as HttpWebResponse;
                                    Console.WriteLine();
                                }

                                //string responseUri = response.RequestMessage.RequestUri.ToString();
                                string responseUri = response.Headers.Location.ToString();


                                col.Value = responseUri;
                                col.Hyperlink.ExternalAddress = new Uri(responseUri);
                            }
                        });
                        //    Console.WriteLine(colsAt10Pos);
                    }

                    wb.Save();
                    MessageBox.Show("Готово");

                }
                else
                {
                    MessageBox.Show("Файл не был выбран!");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, ex.Message);
            }


        }
    }
}