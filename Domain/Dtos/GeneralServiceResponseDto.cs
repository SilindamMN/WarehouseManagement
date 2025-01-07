namespace Domain.Dtos
{
  /// <summary>
  /// Represents the general service response DTO, containing information about the success status,
  /// status code, and a message for the service response.
  /// </summary>
  public class GeneralServiceResponseDto
  {
    /// <summary>
    /// Gets or sets a value indicating whether the service operation succeeded.
    /// </summary>
    public bool IsSucceed { get; set; }

    /// <summary>
    /// Gets or sets the status code representing the result of the service operation.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the service response, providing more details about the result.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="GeneralServiceResponseDto"/> with the specified success status, 
    /// status code, and message.
    /// </summary>
    /// <param name="isSucceed">Indicates whether the service operation succeeded.</param>
    /// <param name="statusCode">The status code of the service operation.</param>
    /// <param name="message">The message providing more details about the operation result.</param>
    /// <returns>A new <see cref="GeneralServiceResponseDto"/> instance.</returns>
    public static GeneralServiceResponseDto CreateResponse(bool isSucceed, int statusCode, string message)
    {
      return new GeneralServiceResponseDto()
      {
        IsSucceed = isSucceed,
        StatusCode = statusCode,
        Message = message
      };
    }
  }
}