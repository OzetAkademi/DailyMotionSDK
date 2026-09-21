using DailymotionSDK.Models.Requests;
using DailymotionSDK.Models.Responses;
using System.Numerics;

namespace DailymotionSDK.Interfaces;

/// <summary>
/// Interface IUpload
/// </summary>
public interface IUpload
{
    /// <summary>
    /// Start an upload session (no JSON body). 
    /// The response contains upload_url(POST the file here) and progress_url(poll until complete). 
    /// Call this before attaching the file to a video record. 
    /// Requires video.manage scope and a valid Bearer token.
    /// </summary>
    /// <param name="fileUploadRequest">The file upload request.</param>
    /// <returns>Task{FileUpload}.</returns>
    Task<FileUpload> UploadAsync(FileUploadRequest fileUploadRequest);

    /// <summary>
    /// Upload using multipart/form-data.
    /// </summary>
    /// <param name="streamUploadRequest">The stream upload request.</param>
    /// <returns>Task{FileUpload}.</returns>
    Task<FileUpload> UploadAsync(FileStreamUploadRequest streamUploadRequest);
}