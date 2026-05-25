namespace QEX_Server.Dtos
{
    public record ExtensionListItemDto(
    string ExtensionId,
    string Name,
    string Description,
    string Author,
    string LatestVersion
    );

    public record ExtensionDetailsDto(
        string ExtensionId,
        string Name,
        string Description,
        string Author,
        string Version,
        string ManifestJson,
        string PackageUrl
    );

    public record PublishExtensionRequest(
        string ExtensionId,
        string Name,
        string Description,
        string Author,
        string Version,
        string ManifestJson,
        string PackageUrl
    );

    public record InstallRequest(string ExtensionId, string Version);
    public record InstallResponse(bool Success, string Message);
}
