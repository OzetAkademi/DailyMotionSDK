using System.ComponentModel.DataAnnotations;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class FileStreamUploadRequest.
    /// </summary>
    public class FileStreamUploadRequest
    {
        /// <summary>
        /// File to upload.
        /// </summary>
        /// <value>The stream.</value>
        [Required(ErrorMessage = "Stream is required.")]
        public Stream? Stream { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [Required(ErrorMessage = "FileName is required.")]
        public string? FileName { get; set; }
    }
}