using OAuth.Client;

namespace OAuth.Client500px
{
    public interface IOAuth500pxResources : IOAuthResources
    {
        int MaxPhotosPerPage { get; }
        string UserUrl { get; }
        string PhotosUrl { get; }
    }
}