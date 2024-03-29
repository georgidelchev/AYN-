﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Administration.Users;
using AYN.Web.ViewModels.Users;

namespace AYN.Services.Data.Interfaces;

public interface IUsersService
{
    Task<IEnumerable<T>> GetAll<T>();

    Task<T> GetProfileDetails<T>(string id);

    Task Follow(string followerId, string followeeId);

    Task Unfollow(string followerId, string followeeId);

    Task<IEnumerable<T>> GetFollowers<T>(string userId);

    Task<IEnumerable<T>> GetFollowings<T>(string userId);

    bool IsFollower(string followerId, string followeeId);

    bool IsUserExisting(string userId);

    Task<string> GenerateDefaultAvatar(string firstName, string lastName);

    Task<string> GenerateDefaultThumbnail(string firstName, string lastName);

    Task<T> GetByIdAsync<T>(string id);

    Task EditAsync(EditUserViewModel model, string wwwRootPath);

    Task<IEnumerable<T>> GetSuggestionPeople<T>(string userId, string openedUserId);

    Tuple<int, int, int> GetCounts();

    Task Ban(BanUserInputModel input, string userId);

    Task Unban(string userId);

    string GetIdByUsername(string username);

    bool IsEmailTaken(string email);
}
