namespace image_categorizer.MVVM.Model
{
    class ImageDetails
    {
        public ImageDetails()
        {
        }

        public string? DateTaken { get; set; }
        public string? TimeTaken { get; set; }
        public string? Location { get; set; }
        public string? CameraModel { get; set; }
        public string? Format { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? IsoDateTime { get; set; }

        public ImageDetails(string? dateTaken, string? timeTaken, string? location, string? cameraModel, string? format, double? latitude, double? longitude, string? filePath, string? fileName, string? isoDateTime)
        {
            DateTaken = dateTaken;
            TimeTaken = timeTaken;
            Location = location;
            CameraModel = cameraModel;
            Format = format;
            Latitude = latitude;
            Longitude = longitude;
            FilePath = filePath;
            FileName = fileName;
            IsoDateTime = isoDateTime;
        }
    }
}
