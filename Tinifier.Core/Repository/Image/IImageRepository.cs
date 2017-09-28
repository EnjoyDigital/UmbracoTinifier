﻿using System.Collections.Generic;

namespace Tinifier.Core.Repository.Image
{
    /// <summary>
    /// Repository for images with custom Methods
    /// </summary>
    /// <typeparam name="Media">Media type</typeparam>
    public interface IImageRepository<Media>
    {
        void Update(int imageId);

        IEnumerable<Media> GetOptimizedItems();

        IEnumerable<Media> GetItemsFromFolder(int folderId);

        int AmounthOfItems();

        int AmounthOfOptimizedItems();

        Media Get(string path);
    }
}
