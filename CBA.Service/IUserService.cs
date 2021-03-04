using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public interface IUserService
    {
        Task<ManageUserRolesViewModel> ListUserRolesAsync(string userId);
        void EditUserRolesAsync(string userId, IList<UserRolesViewModel> userRolesViewModels, CBAUser currentUser);
        Task<List<CBAUser>> ListUsersExceptSpecifiedUserAsync(CBAUser user);
    }
}
