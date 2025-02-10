﻿using School.DAL.Models;
using School.PL.Models;

namespace School.PL.Services
{
    public interface IClassesServices
    {
        Task<IEnumerable<ClassRoomViewModel>> GetAllClassAsync();
        Task<ClassRoomViewModel> GetIdClassRoomAsync(Guid? Id);
        Task CreateClassRoomAsync(ClassRoomViewModel model);
        Task UpdateClassRoom(ClassRoomViewModel model);
        Task DeleteClassRoom(ClassRoomViewModel model);
        Task SaveData();
    }
}
