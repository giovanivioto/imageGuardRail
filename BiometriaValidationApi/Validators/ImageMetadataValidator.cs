using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiometriaValidacaoAPI.Validations
{
    public static class ImageMetadataValidator
    {
        public static void ValidarMetadados(string imagemBase64)
        {
            var imagemBytes = Convert.FromBase64String(imagemBase64);

            var directories = ImageMetadataReader.ReadMetadata(imagemBytes);
            var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

            if (exifDirectory == null)
                throw new ArgumentException("Metadados EXIF não encontrados na imagem.");

            ValidarDataCaptura(exifDirectory);
            ValidarGPS(exifDirectory);
        }

        private static void ValidarDataCaptura(ExifIfd0Directory exifDirectory)
        {
            var dateTime = exifDirectory.GetDescription(ExifIfd0Directory.TagDateTime);

            if (string.IsNullOrEmpty(dateTime))
                throw new ArgumentException("Data de captura não encontrada nos metadados da imagem.");

            if (!DateTime.TryParse(dateTime, out DateTime dataCaptura) || dataCaptura > DateTime.UtcNow)
                throw new ArgumentException("Data de captura inválida nos metadados da imagem.");
        }

        private static void ValidarGPS(ExifIfd0Directory exifDirectory)
        {
            var latitude = exifDirectory.GetDescription(ExifIfd0Directory.TagGpsLatitude);
            var longitude = exifDirectory.GetDescription(ExifIfd0Directory.TagGpsLongitude);

            if (string.IsNullOrEmpty(latitude) || string.IsNullOrEmpty(longitude))
                throw new ArgumentException("Coordenadas GPS não encontradas nos metadados da imagem.");

            if (!IsValidLatitudeLongitude(latitude, longitude))
                throw new ArgumentException("Coordenadas GPS inválidas nos metadados da imagem.");
        }

        private static bool IsValidLatitudeLongitude(string latitude, string longitude)
        {
            if (double.TryParse(latitude, out double lat) && double.TryParse(longitude, out double lon))
            {
                return lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;
            }
            return false;
        }
    }
}
