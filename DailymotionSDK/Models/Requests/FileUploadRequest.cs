using System.ComponentModel.DataAnnotations;

namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class FileUploadRequest.
    /// </summary>
    public class FileUploadRequest
    {
        /// <summary>
        /// File to upload.
        /// </summary>
        /// <value>The file path.</value>
        [Required(ErrorMessage = "File path is required.")]
        public string? FilePath { get; set; }
    }
}