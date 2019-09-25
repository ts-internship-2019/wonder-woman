using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Web.Mvc;
using A = DocumentFormat.OpenXml.Drawing;
using Comment = iWasHere.Domain.Models.Comment;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace iWasHere.Domain.Service
{
    public class HomeService
    {
        private readonly DatabaseContext _dbContext;
        private IHostingEnvironment _environment;
        public HomeService(DatabaseContext databaseContext, IHostingEnvironment environment)
        {
            _dbContext = databaseContext;
            _environment = environment;
        }
        public List<LandmarkListModel> GetLandmarkListModels()
        {
            List<LandmarkListModel> landmarkList = _dbContext.Landmark.Select(a => new LandmarkListModel()
            {
                LandmarkId = a.LandmarkId,
                Name = a.Name
            }).ToList();
            return landmarkList;
        }
        public LandmarkModel GetLandmarkById(int id)
        {
            var landmark = _dbContext.Landmark.First(a => a.LandmarkId == id);
            LandmarkModel selectedlandmark = new LandmarkModel()
            {
                LandmarkId = landmark.LandmarkId,
                Code = landmark.Code,
                Name = landmark.Name,
                Descr = landmark.Descr,
                ConstructionTypeId = landmark.ConstructionTypeId,
                HistoricalPeriodTypeId = landmark.HistoricalPeriodTypeId,
                LandmarkTypeId = landmark.LandmarkTypeId,
                Latitude = landmark.Latitude,
                Longitude = landmark.Longitude,
                CountryId = landmark.CountryId,
                CountyId = landmark.CountyId,
                CityId = landmark.CityId
            };
            return selectedlandmark;
        }

       public List<String> GetImagesForLandmarkId(int id)
        {
            List<Photo> photopaths = _dbContext.Photo.Where(a => a.LandmarkId == id).Select(a => new Photo()
            {
                ImagePath = a.ImagePath,
            }).ToList();
            List<String> filepaths = new List<String>();
            foreach (Photo ph in photopaths)
            {
                filepaths.Add(ph.ImagePath);
            }
            return filepaths;
        }
        public List<String> GetLocationForLandmarkId(int id)
        {
            Landmark landmark = _dbContext.Landmark.First(a => a.LandmarkId == id);
            var cityId = landmark.CityId;
            City city = _dbContext.City.First(a => a.CityId == cityId);
            County county = _dbContext.County.First(a => a.CountyId == city.CountyId);
            Country country = _dbContext.Country.First(a => a.CountryId == county.CountryId);
            List<String> location = new List<String>();
            location.Add(country.Name + ", " + county.Name + ", " + city.Name);
            return location;
        }
        public List<String> GetConstructionForLandmarkId(int id)
        {
            List<String> constructionstring = new List<String>();
            Landmark landmark = _dbContext.Landmark.First(a => a.LandmarkId == id);
            if (landmark.ConstructionTypeId == null)
            {
                constructionstring.Add("");
                constructionstring.Add("");
                return constructionstring;
            }
            DictionaryConstructionType construction = _dbContext.DictionaryConstructionType.First(a => a.ConstructionTypeId == landmark.ConstructionTypeId);
            constructionstring.Add(construction.Name);
            constructionstring.Add(construction.Description);
                return constructionstring;
        }
        public List<String> GetLandmarktypeForLandmarkId(int id)
        {
            List<String> lmkTypestring = new List<String>();
            Landmark landmark = _dbContext.Landmark.First(a => a.LandmarkId == id);
            if (landmark.LandmarkTypeId == null)
            {
                lmkTypestring.Add("");
                lmkTypestring.Add("");
                return lmkTypestring;
            }
            DictionaryLandmarkType landmarkType = _dbContext.DictionaryLandmarkType.First(a => a.LandmarkTypeId == landmark.LandmarkTypeId);
            lmkTypestring.Add(landmarkType.Name);
            lmkTypestring.Add(landmarkType.Description);
                return lmkTypestring;
        }
        //public void UpdateLandmark(LandmarkModel lm, out string errorMessage,out int id)

        public List<Comment> GetCommentsForLandmarkId(int id)
        {
            List<Comment> comm = _dbContext.Comment.Where(a => a.LandmarkId == id).Select(a => new Comment()
            {
                OwnerName = a.OwnerName,
                RatingValue = a.RatingValue,
                Title = a.Title,
                Text = a.Text
            }).ToList();
            return comm;
        }

        public void UpdateLandmark(LandmarkModel lm, out string errorMessage, out int id)
        {

            Landmark landmark = new Landmark();
            if (lm.LandmarkId != 0)
                landmark.LandmarkId = lm.LandmarkId;
            if (!(string.IsNullOrWhiteSpace(lm.Code)))
                landmark.Code = lm.Code;
            if (!(string.IsNullOrWhiteSpace(lm.Name)))
                landmark.Name = lm.Name;
            if (!string.IsNullOrWhiteSpace(lm.Descr))
                landmark.Descr = lm.Descr;
            if (lm.ConstructionTypeId != null)
                landmark.ConstructionTypeId = lm.ConstructionTypeId;
            if (lm.LandmarkTypeId != null)
                landmark.LandmarkTypeId = lm.LandmarkTypeId;
            if (lm.Latitude != null)
                landmark.Latitude = lm.Latitude;
            if (lm.Longitude != null)
                landmark.Longitude = lm.Longitude;
            if (lm.CountryId != null)
                landmark.CountryId = lm.CountryId;
            if (lm.CountyId != null)
                landmark.CountyId = lm.CountyId;
            if (lm.CityId != null)
                landmark.CityId = lm.CityId;
             if(lm.CityId!=null && lm.CountyId==null)
            {
                City city = _dbContext.City.First(a => a.CityId == lm.CityId);
                landmark.CountyId = city.CountyId;
                County county = _dbContext.County.First(a => a.CountyId == city.CountyId);
                landmark.CountryId = county.CountryId;

            }
            errorMessage = "";
            if (lm.LandmarkId == 0)
            {
                _dbContext.Landmark.Add(landmark);
            }
            else
            {
                _dbContext.Landmark.Update(landmark);
            }
           
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                errorMessage = "Salvarea/Editarea nu a putut fi efectuata cu succes! Te rog sa mai incearci o data!";
            }
            id = landmark.LandmarkId;
        }

        public void AddComments(Comment cm, out string errorMessage)
        {

            Comment comm = new Comment();
            if (!string.IsNullOrWhiteSpace(cm.OwnerName))
                comm.OwnerName = cm.OwnerName;
            else
                comm.OwnerName = "Anonim";
            if (!string.IsNullOrWhiteSpace(cm.Title))
                comm.Title = cm.Title;
            else
                comm.Title = "No Title!";
            if (!string.IsNullOrWhiteSpace(cm.Text))
                comm.Text = cm.Text;
            if (cm.RatingValue != 0)
                comm.RatingValue = cm.RatingValue;
            comm.LandmarkId = cm.LandmarkId;
                errorMessage = "";
            _dbContext.Comment.Add(comm);
            //try
            {
                _dbContext.SaveChanges();
            }
            //catch (Exception ex)
            {
                //string var = ex.Message;
                errorMessage = "Salvarea/Editarea nu a putut fi efectuata cu succes! Te rog sa mai incearci o data!";
            }
        }

        public List<DictionaryLandmarkType> GetLandmarks(string text)
        {
            var query = _dbContext.DictionaryLandmarkType.Select(c => new DictionaryLandmarkType()
            {
                LandmarkTypeId = c.LandmarkTypeId,
                Name = c.Name,
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }
        public void SaveImagesDB(string path, int id)
        {
           
            Photo photo = new Photo()
            {
                ImagePath = "~/images/" + path,
                LandmarkId = id
            };

            _dbContext.Photo.Add(photo);

            _dbContext.SaveChanges();

        }
        public string DeleteImagesDB( int id)
        {

            Photo deleted = _dbContext.Photo.First(a => a.PhotoId == id);

            _dbContext.Photo.Remove(deleted);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(ex.ToString()))
                    return ex.ToString();
            }
            return null;

        }

        public List<CityModel> GetCities(string text)
        {
            var query = _dbContext.City.Select(c => new CityModel()
            {
                Id = c.CityId,
                Name = c.Name
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }

        public List<DictionaryConstructionTypeModel> GetConstructions(string text)
        {
            var query = _dbContext.DictionaryConstructionType.Select(c => new DictionaryConstructionTypeModel()
            {
                ConstructionTypeId = c.ConstructionTypeId,
                Name = c.Name
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }

        public void DestroyLandmark(LandmarkListModel landmarkToDestroy,out string errorMessage)
        {
            var db = _dbContext;
            errorMessage = "";
            try
            {
                var landmarks = db.Landmark.Where(pd => pd.LandmarkId == landmarkToDestroy.LandmarkId);

                foreach (var land in landmarks)
                {
                    db.Landmark.Remove(land);
                }

                db.SaveChanges();
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
            }
        }
        public Stream ExportToWord(LandmarkModel model)

        {
            string firstPhoto=null;
            string cityName = null;
            string countyName = null;
            string countryName = null;
            string constructionType = null;
            model.Name = _dbContext.Landmark.Where(x => x.LandmarkId == model.LandmarkId).Select(x => x.Name).FirstOrDefault();

            City city = _dbContext.City.First(a => a.CityId == model.CityId);
            cityName = city.Name;
            County county = _dbContext.County.First(a => a.CountyId == model.CountyId);
            countyName = county.Name;
            Country country = _dbContext.Country.First(a => a.CountryId == model.CountryId);
            countryName = country.Name;
            DictionaryConstructionType construction = _dbContext.DictionaryConstructionType.First(a => a.ConstructionTypeId == model.ConstructionTypeId);
            constructionType = construction.Name;
            if (_dbContext.Photo.Where(a => a.LandmarkId == model.LandmarkId).Count() > 0)
            {
                Photo photo = _dbContext.Photo.First(a => a.LandmarkId == model.LandmarkId);
                firstPhoto = photo.ImagePath;

            }

            var stream = new MemoryStream();

            using (WordprocessingDocument doc = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))

            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                new DocumentFormat.OpenXml.Wordprocessing.Document(new Body()).Save(mainPart);
                if (firstPhoto != null)
                {
                    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                    using (FileStream imgstream = new FileStream(_environment.WebRootPath + firstPhoto.Substring(1), FileMode.Open))
                    {
                        imagePart.FeedData(stream);
                    }

                    AddImageToBody(doc, mainPart.GetIdOfPart(imagePart));
                }
                Body body = mainPart.Document.Body;
                body.Append(
                      new Body(
                      new Paragraph(
                        new Run(
                          new Text("Numele obiectivului: " + model.Name))),                     
                           new Paragraph(
                        new Run(
                          new Text("\n Descrierea atractiei este: " + model.Descr))),
                            
                               new Paragraph(
                        new Run(
                            new Text("\n Tipul de constructie este: " + constructionType))),

                                 new Paragraph(
                        new Run(
                              new Text("\n Orasul: " + cityName))),

                                  new Paragraph(
                        new Run(
                              new Text("\n Judetul: " + countyName))),
                                   new Paragraph(
                        new Run(
                              new Text("\n Tara: " + countryName)))                          

                          ));

                mainPart.Document.Save();
            }



            stream.Seek(0, SeekOrigin.Begin);



            return stream;

        }


        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }


    }
}
