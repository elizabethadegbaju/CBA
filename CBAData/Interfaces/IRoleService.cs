using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface IRoleService
    {
        Task<RoleViewModel> RetrieveRoleDetailAsync(string roleId);
        Task<string> CreateRoleAsync(string name);
        Task EditRoleAsync(string roleId, bool isEnabled, string name);
        Task<bool> CheckRoleExistsAsync(string name);
    }
}
