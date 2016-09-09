using MUMScrum.Model;
using System.Collections.Generic;

namespace MUMScrum.BusinessService
{
    public interface IUserStoryService
    {
        UserStory GetUserStoryById(int Id);
        IEnumerable<UserStory> GetAll();
        void CreateUserStory(UserStory US);
        bool UpdateUserStory(UserStory US, int Id);
        bool DeleteUserStory(int Id);
        IEnumerable<UserStory> GetAllByDeveloperId(int DeveloperId);
        IEnumerable<UserStory> GetAllByTesterId(int TesterId);
        void DeleteRelatedLogs(int id);
    }
}
