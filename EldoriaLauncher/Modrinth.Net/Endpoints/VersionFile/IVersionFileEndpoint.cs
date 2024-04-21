﻿using Modrinth.Exceptions;
using Modrinth.Models.Enums;

namespace Modrinth.Endpoints.VersionFile;

/// <summary>
///     Version file endpoints
/// </summary>
public interface IVersionFileEndpoint
{
    /// <summary>
    ///     Get specific version by file hash
    /// </summary>
    /// <param name="hash">The hash of the file, considering its byte content, and encoded in hexadecimal</param>
    /// <param name="hashAlgorithm"> The hash algorithm used to generate the hash </param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation </param>
    /// <returns></returns>
    /// <exception cref="ModrinthApiException"> Thrown when the API returns an error or the request fails </exception>
    Task<Models.Version> GetVersionByHashAsync(string hash,
        HashAlgorithm hashAlgorithm = HashAlgorithm.Sha1, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a version by its file hash
    /// </summary>
    /// <param name="hash"> The hash of the file, considering its byte content, and encoded in hexadecimal </param>
    /// <param name="hashAlgorithm"> The hash algorithm used to generate the hash </param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation </param>
    /// <returns></returns>
    Task DeleteVersionByHashAsync(string hash,
        HashAlgorithm hashAlgorithm = HashAlgorithm.Sha1, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get multiple versions by file hash
    /// </summary>
    /// <param name="hashes"> The hashes of the files, considering their byte content, and encoded in hexadecimal </param>
    /// <param name="hashAlgorithm"> The hash algorithm used to generate the hash </param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation </param>
    /// <returns> A dictionary of hashes and their respective versions </returns>
    Task<IDictionary<string, Models.Version>> GetMultipleVersionsByHashAsync(string[] hashes,
        HashAlgorithm hashAlgorithm = HashAlgorithm.Sha1, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the latest version of a projects by a file hash
    /// </summary>
    /// <param name="hash"> The hash of the file, considering its byte content, and encoded in hexadecimal </param>
    /// <param name="hashAlgorithm"> The hash algorithm used to generate the hash </param>
    /// <param name="loaders"> The loaders to filter by </param>
    /// <param name="gameVersions"> The game versions to filter by </param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation </param>
    /// <returns> The latest version of a project, that matches the filters </returns>
    Task<Models.Version> GetLatestVersionByHashAsync(string hash, HashAlgorithm hashAlgorithm,
        string[] loaders, string[] gameVersions, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the latest version of multiple projects by a file hash
    /// </summary>
    /// <param name="hashes"> The hashes of the files, considering their byte content, and encoded in hexadecimal </param>
    /// <param name="hashAlgorithm"> The hash algorithm used to generate the hash </param>
    /// <param name="loaders"> The loaders to filter by </param>
    /// <param name="gameVersions"> The game versions to filter by </param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation </param>
    /// <returns> A dictionary of hashes and their respective latest versions that match the filters </returns>
    Task<IDictionary<string, Models.Version>> GetMultipleLatestVersionsByHashAsync(string[] hashes,
        HashAlgorithm hashAlgorithm,
        string[] loaders, string[] gameVersions, CancellationToken cancellationToken = default);
}