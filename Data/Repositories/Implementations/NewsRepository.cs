using System.Data.SqlClient;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Helper;
using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Services.Interfaces;
using static PremierLeague_Backend.Helper.SqlCommands.NewsCommands;

namespace PremierLeague_Backend.Data.Repositories.Implementations;

public class NewsRepository : INewsRepository
{
    private readonly IExecute execute;
    private readonly IFileStorageService storageService;
    private string _newsFolder => "upload/news";

    public NewsRepository(IExecute execute, IFileStorageService storageService)
    {
        this.execute = execute;
        this.storageService = storageService;
    }

    public async Task<bool> AddNewsAsync(NewsDto newsDto)
    {
        try
        {
            if (newsDto is null) throw new ArgumentNullException(nameof(newsDto));
            if (!string.Equals(newsDto.ImageFile?.FileName, "none.png", StringComparison.OrdinalIgnoreCase))
                newsDto.ImageUrl = await storageService.SavePhotoAsync(newsDto.ImageFile!, _newsFolder);

            var cmd = new SqlCommand();
            cmd.CommandText = AddNewsCommand;
            cmd.Parameters.AddWithValue("@Title", newsDto.Title);
            cmd.Parameters.AddWithValue("@Subtitle", newsDto.Subtitle);
            cmd.Parameters.AddWithValue("@Content", newsDto.Content);
            cmd.Parameters.AddWithValue("@ReferenceUrl", newsDto.ReferenceUrl);
            cmd.Parameters.AddWithValue("@VideoReferenceUrl", newsDto.VideoReferenceUrl);
            cmd.Parameters.AddWithValue("@NewsTagId", newsDto.NewsTagId);
            cmd.Parameters.AddWithValue("@ImageUrl", newsDto.ImageUrl);
            cmd.Parameters.AddWithValue("@PublishedDate", newsDto.PublishedDate);
            cmd.Parameters.AddWithValue("@ExpireDate", newsDto.ExpiryDate);
            cmd.Parameters.AddWithValue("@AuthorId", newsDto.AuthorId);
            cmd.Parameters.AddWithValue("@MatchId", newsDto.MatchId);
            cmd.Parameters.AddWithValue("@ClubId", newsDto.ClubId);
            cmd.Parameters.AddWithValue("@IsActive", newsDto.IsActive);
            cmd.Parameters.AddWithValue("@IsFeatured", newsDto.IsActive);
            cmd.Parameters.AddWithValue("@IsVideo", newsDto.IsVideo);
            cmd.Parameters.AddWithValue("@IsQuizzes", newsDto.IsQuizzes);
            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database add news error: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteNewsAsync(int newsId, string? imageUrl)
    {
        var cmd = new SqlCommand();
        cmd.CommandText = DeleteNewsCommand;
        cmd.Parameters.AddWithValue("@NewsId", newsId);
        try
        {
            var scalar = await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
            if (scalar)
            {
                if (!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrWhiteSpace(imageUrl))
                {
                    await storageService.DeleteFileAsync(Path.Combine(_newsFolder, imageUrl));
                }
            }

            return scalar;
        }
        catch (Exception ex)
        {
            throw new Exception($"Database delete news error: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<NewsDetailDto>> GetAllNewsDetailAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetAllNewsDetailCommand;
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var newsDetailDtos = new List<NewsDetailDto>();
            if (rdr is not null)
            {
                do
                {
                    newsDetailDtos.Add(new NewsDetailDto()
                    {
                        NewsId = rdr.SafeGetInt("NewsId"),
                        NewsTagId = rdr.SafeGetInt("NewsTagId"),
                        NewsTagName = rdr.SafeGetString("NewsTagName"),
                        Title = rdr.SafeGetString("Title"),
                        Subtitle = rdr.SafeGetString("Subtitle"),
                        Content = rdr.SafeGetString("Content"),
                        ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                        Author = rdr.SafeGetString("Author"),
                        PublishedDate = rdr.SafeGetString("PublishedDate"),
                        ImageUrl = rdr.SafeGetString("ImageUrl"),
                        ExpiryDate = rdr.SafeGetString("ExpiryDate"),
                        IsActive = rdr.SafeGetBoolean("IsActive")
                    });
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));

            }
            return newsDetailDtos;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database fetching all news detail error: {ex.Message}", ex);
        }
    }

    public async Task<NewsDto?> GetNewsByIdAsync(int newsId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetNewsByIdCommand;
            cmd.Parameters.AddWithValue("@NewsId", newsId);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var newsDto = new NewsDto();
            if (rdr is not null)
            {
                do
                {
                    newsDto = new NewsDto()
                    {
                        NewsId = rdr.SafeGetInt("NewsId"),
                        NewsTagId = rdr.SafeGetInt("NewsTagId"),
                        Title = rdr.SafeGetString("Title"),
                        Subtitle = rdr.SafeGetString("Subtitle"),
                        Content = rdr.SafeGetString("Content"),
                        ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                        AuthorId = rdr.SafeGetString("AuthorId"),
                        PublishedDate = rdr.SafeGetDateTime("PublishedDate"),
                        ImageUrl = rdr.SafeGetString("ImageUrl"),
                        ExpiryDate = rdr.SafeGetDateTime("ExpiryDate"),
                        IsActive = rdr.SafeGetBoolean("IsActive"),
                        MatchId = rdr.SafeGetInt("MatchId"),
                        ClubId = rdr.SafeGetInt("ClubId")
                    };
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));

            }
            return newsDto;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database fetching news by id error: {ex.Message}", ex);
        }
    }

    public async Task<bool> UpdateNewsAsync(NewsDto newsDto)
    {
        try
        {
            if (newsDto is null) throw new ArgumentNullException(nameof(newsDto));
            if(newsDto.ImageFile is not null || newsDto.ImageFile?.Length > 0)
            {
                if (!string.Equals(newsDto.ImageFile?.FileName, "none.png", StringComparison.OrdinalIgnoreCase))
                    newsDto.ImageUrl = await storageService.SavePhotoAsync(newsDto.ImageFile!, _newsFolder);
            }

            var cmd = new SqlCommand();
            cmd.CommandText = UpdateNewsCommand;
            cmd.Parameters.AddWithValue("@NewsId", newsDto.NewsId);
            cmd.Parameters.AddWithValue("@Title", newsDto.Title);
            cmd.Parameters.AddWithValue("@Subtitle", newsDto.Subtitle);
            cmd.Parameters.AddWithValue("@Content", newsDto.Content);
            cmd.Parameters.AddWithValue("@ReferenceUrl", newsDto.ReferenceUrl);
            cmd.Parameters.AddWithValue("@VideoReferenceUrl", newsDto.VideoReferenceUrl);
            cmd.Parameters.AddWithValue("@NewsTagId", newsDto.NewsTagId);
            cmd.Parameters.AddWithValue("@ImageUrl", newsDto.ImageUrl);
            cmd.Parameters.AddWithValue("@PublishedDate", newsDto.PublishedDate);
            cmd.Parameters.AddWithValue("@ExpireDate", newsDto.ExpiryDate);
            cmd.Parameters.AddWithValue("@AuthorId", newsDto.AuthorId);
            cmd.Parameters.AddWithValue("@MatchId", newsDto.MatchId);
            cmd.Parameters.AddWithValue("@ClubId", newsDto.ClubId);
            cmd.Parameters.AddWithValue("@IsActive", newsDto.IsActive);
            cmd.Parameters.AddWithValue("@IsFeatured", newsDto.IsActive);
            cmd.Parameters.AddWithValue("@IsVideo", newsDto.IsVideo);
            cmd.Parameters.AddWithValue("@IsQuizzes", newsDto.IsQuizzes);
            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database update news error: {ex.Message}", ex);
        }
    }
}
