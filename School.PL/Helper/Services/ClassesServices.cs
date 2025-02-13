using School.BLL.Interfaces;
using School.DAL.Models;
using School.PL.Models;

namespace School.PL.Helper.Services
{
    public class ClassesServices : IClassesServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassesServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClassRoomViewModel>> GetAllClassAsync()
        {
            var classes = await _unitOfWork.ClassesRepository.GetAllAsync();

            // Manually map the Class entities to ClassRoomViewModel
            var classRoomViewModels = classes.Select(c => new ClassRoomViewModel
            {
                Id = c.Id,
                Name = c.Name,
            });

            return classRoomViewModels;
        }

        public async Task<ClassRoomViewModel> GetIdClassRoomAsync(Guid? Id)
        {
            // Fetch the class from the repository
            var classEntity = await _unitOfWork.ClassesRepository.GetByIdAsync(Id.Value);

            if (classEntity == null)
            {
                return null; // Or handle this scenario as per your requirements
            }

            // Manually map from the class entity to the view model
            var classRoomViewModel = new ClassRoomViewModel
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                // Map other properties as necessary
            };

            return classRoomViewModel;
        }
        //public async Task<ClassRoomViewModel> GetIdClassRoomAsync(Guid? Id)
        //{
        //    return await _unitOfWork.ClassesRepository.GetByIdAsync(Id.Value);
        //}

        public async Task CreateClassRoomAsync(ClassRoomViewModel model)
        {
            // Manually map the properties from ClassRoomViewModel to ClassRoom
            var ClassRoom = new Classes
            {
                Id = model.Id,
                Name = model.Name,
            };

            // Add the mapped ClassRoom entity to the repository
            await _unitOfWork.ClassesRepository.AddAsync(ClassRoom);
            await _unitOfWork.SaveDataAsync();
        }

        public async Task UpdateClassRoom(ClassRoomViewModel model)
        {
            var ClassRoom = new Classes
            {
                Id = model.Id,
                Name = model.Name,
            };
            _unitOfWork.ClassesRepository.Update(ClassRoom);
            await _unitOfWork.SaveDataAsync();
        }

        public async Task DeleteClassRoom(ClassRoomViewModel model)
        {
            // Retrieve the existing entity from the database
            var classRoomEntity = await _unitOfWork.ClassesRepository.GetByIdAsync(model.Id);

            // If the entity exists, delete it
            if (classRoomEntity != null)
            {
                _unitOfWork.ClassesRepository.Delete(classRoomEntity);

                // Saving the changes to the database asynchronously
                await _unitOfWork.SaveDataAsync();
            }
            else
            {
                // Handle case when the entity is not found (optional)
                throw new Exception("Classroom not found.");
            }
        }


        public async Task SaveData()
        {
            await _unitOfWork.SaveDataAsync();
        }
    }
}
