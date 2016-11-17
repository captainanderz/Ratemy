using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MySql.Data.MySqlClient;

namespace RateMy.com.Models
{
    public class CommonHelpers
    {
        public ImageForDb CreateImageForDb(string name, string email, HttpPostedFileBase uploadFile)
        {
            var uploadFileAsStream = uploadFile.InputStream;
            name = FirstLetterUppercase(name); // First letter in name to Uppercase
            var imageForDb = new ImageForDb(name, email, 0, 0, 0, false, uploadFile.FileName, uploadFile.ContentType,
                uploadFileAsStream);
            return imageForDb;
        }

        public string FirstLetterUppercase(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public List<ImageForWeb> GetImagesForWebFromDb(bool admin)
        {
            var listOfImageForWeb = new List<ImageForWeb>();

            var connectionString = ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString;
            using (var con = new MySqlConnection(connectionString))
            {
                const string nameQuery = "SELECT * FROM Images";

                using (var cmd = new MySqlCommand(nameQuery))
                {
                    cmd.Connection = con;
                    con.Open(); // Open connection

                    var cmdtest = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", con);
                        // Setting timeout on mysqlServer
                    cmdtest.ExecuteNonQuery();

                    var reader = cmd.ExecuteReader();
                    while (reader.Read()) // Start reader
                    {
                        var id = Convert.ToInt32(reader["id"].ToString());
                        var name = reader["Name"].ToString();
                        var email = reader["Email"].ToString();
                        var upVotes = Convert.ToInt32(reader["UpVotes"].ToString());
                        var downVotes = Convert.ToInt32(reader["DownVotes"].ToString());
                        var reports = Convert.ToInt32(reader["Reports"].ToString());
                        var shown = Convert.ToBoolean(reader["Shown"].ToString());
                        var imageName = reader["ImageName"].ToString();
                        var imageType = reader["ImageType"].ToString();

                        var imageFileBytes = (byte[]) reader["ImageFile"];

                        if (imageFileBytes == null) continue;
                        var imageFile = Image.FromStream(new MemoryStream(imageFileBytes));

                        if (admin && shown == false)
                        {
                            // Add image info to returning list
                            listOfImageForWeb.Add(new ImageForWeb(id, name, email, upVotes, downVotes, reports, imageName,
                                imageType, imageFile, GetFullImageFromFolder(id.ToString(), name)));
                        }
                        else if (admin == false && shown)
                        {
                            // Add image info to returning list
                            listOfImageForWeb.Add(new ImageForWeb(id, name, email, upVotes, downVotes, reports, imageName,
                                imageType, imageFile, GetFullImageFromFolder(id.ToString(), name)));
                        }

                        // Save save image to folder if not already there
                        SaveNewImagesFromDbToContentFolder(id.ToString(), name, imageFile);
                    }

                    reader.Close(); // Stop reader
                    con.Close(); // Close connection
                }
            }
            return listOfImageForWeb;
        }

        public void SaveNewImagesFromDbToContentFolder(string id, string name, Image image)
        {
            var savePath = HostingEnvironment.MapPath("~/Content/images/Fulls/") + id + name + ".Jpeg";
            if (!File.Exists(savePath))
                image.Save(savePath, ImageFormat.Jpeg);
        }

        public IEnumerable<string> GetAllFullImagesFromFolder()
        {
            var images = Directory.EnumerateFiles(HostingEnvironment.MapPath("~/Content/images/Fulls/"))
                .Select(fn => "~/Content/images/Fulls/" + Path.GetFileName(fn));

            // Trim for HTML
            images = images.Select(x => x.TrimStart(new []{'~', '/'}));
            return images;
        }

        public string GetFullImageFromFolder(string id, string name)
        {
            var images = GetAllFullImagesFromFolder();
            var image = images.FirstOrDefault(x => x.Contains(id + name));

            return image;
        }
    }
}