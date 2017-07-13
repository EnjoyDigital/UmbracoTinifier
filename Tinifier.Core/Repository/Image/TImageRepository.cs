﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tinifier.Core.Repository.Common;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Tinifier.Core.Repository.Image
{
    public class TImageRepository : IEntityReader<Media>, IImageRepository<Media>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IMediaService _mediaService;
        private readonly UmbracoDatabase _database;
        private readonly List<Media> mediaList = new List<Media>();

        public TImageRepository()
        {
            _mediaService = ApplicationContext.Current.Services.MediaService;
            _database = ApplicationContext.Current.DatabaseContext.Database;
            _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
        }

        /// <summary>
        /// Get all media
        /// </summary>
        /// <returns>IEnumerable of Media</returns>
        public IEnumerable<Media> GetAll()
        {
            var mediaItems = _mediaService.GetMediaOfMediaType(_contentTypeService.GetMediaType("image").Id);

            return mediaItems.Select(item => item as Media).ToList();
        }

        /// <summary>
        /// Get Media by Id
        /// </summary>
        /// <param name="id">Media Id</param>
        /// <returns>Media</returns>
        public Media GetByKey(int id)
        {
            var mediaItem = _mediaService.GetById(id) as Media;

            return mediaItem;
        }

        /// <summary>
        /// Update Media
        /// </summary>
        /// <param name="id">Media Id</param>
        public void UpdateItem(int id)
        {
            var mediaItem = _mediaService.GetById(id) as Media;

            if (mediaItem != null)
            {
                mediaItem.UpdateDate = DateTime.UtcNow;

                _mediaService.Save(mediaItem);
            }
        }

        /// <summary>
        /// Get Optimized Images
        /// </summary>
        /// <returns>IEnumerable of Media</returns>
        public IEnumerable<Media> GetOptimizedItems()
        {
            var query = new Sql("SELECT ImageId FROM TinifierResponseHistory WHERE IsOptimized = 'true'");
            var historyIds = _database.Fetch<int>(query);

            var mediaItems = _mediaService.
                             GetMediaOfMediaType(_contentTypeService.GetMediaType("image").Id).
                             Where(item => historyIds.Contains(item.Id));

            return mediaItems.Select(item => item as Media).ToList();
        }

        /// <summary>
        /// Get Media from folder
        /// </summary>
        /// <param name="folderId">Folder Id</param>
        /// <returns>IEnumerable of Media</returns>
        public IEnumerable<Media> GetItemsFromFolder(int folderId)
        {
            var imagesFolder = _mediaService.GetById(folderId);
            var items = imagesFolder.Children();

            if (items.Any())
            {
                foreach (var media in items)
                {
                    if (media.ContentType.Alias == "Image")
                    {
                        mediaList.Add(media as Media);
                    }
                } 
                foreach(var media in items)
                {
                    if (media.ContentType.Alias == "Folder")
                    {
                        GetItemsFromFolder(media.Id);
                    }
                }  
            }

            return mediaList;
        }

        /// <summary>
        /// Get Count of Images
        /// </summary>
        /// <returns>Number of Images</returns>
        public int AmounthOfItems()
        {
            var mediaItems = _mediaService.GetMediaOfMediaType(_contentTypeService.GetMediaType("image").Id);
            var numberOfItems = mediaItems.Count();

            return numberOfItems;
        }

        /// <summary>
        /// Get Count of Optimized Images
        /// </summary>
        /// <returns>Number of optimized Images</returns>
        public int AmounthOfOptimizedItems()
        {
            var query = new Sql("SELECT ImageId FROM TinifierResponseHistory WHERE IsOptimized = 'true'");
            var historyIds = _database.Fetch<int>(query);

            var mediaItems = _mediaService.
                             GetMediaOfMediaType(_contentTypeService.GetMediaType("image").Id).
                             Where(item => historyIds.Contains(item.Id));

            return mediaItems.Count();
        }

        /// <summary>
        /// Get Media By path
        /// </summary>
        /// <param name="path">relative path</param>
        /// <returns>Media</returns>
        public Media GetByPath(string path)
        {
            var mediaItem = _mediaService.GetMediaByPath(path) as Media;

            return mediaItem;
        }
    }
}
