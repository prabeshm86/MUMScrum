using System.Collections.Generic;
using MUMScrum.Model;
using MUMScrum.DataAccess;
using System.Web;
using System;


namespace MUMScrum.BusinessService
{
    public class UserStoryService : IUserStoryService
    {
        private IUnitOfWork UnitOfWork = new UnitOfWork();


        public void CreateUserStory(UserStory US)
        {
            UnitOfWork.UserStoryRepository.Insert(US);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<UserStory> GetAll()
        {
            return UnitOfWork.UserStoryRepository.Get();
        }

        public IEnumerable<UserStory> GetAllByDeveloperId(int DeveloperId)
        {
            return UnitOfWork.UserStoryRepository.Get(u => u.DevelopedId == DeveloperId);
        }

        public IEnumerable<UserStory> GetAllByTesterId(int TesterId)
        {
            return UnitOfWork.UserStoryRepository.Get(u => u.TesterId == TesterId);
        }

        public bool DeleteUserStory(int Id)
        {
            UserStory US = UnitOfWork.UserStoryRepository.GetByID(Id);
            if (US.Sprint != null)
            {
                if (US.Sprint.SprintRunning)
                    throw new Exception("UserStory can't be deleted because the related Sprint is running!");
            }
            UnitOfWork.UserStoryRepository.Delete(Id);
            return UnitOfWork.SaveChanges();
        }

        public UserStory GetUserStoryById(int Id)
        {
            return UnitOfWork.UserStoryRepository.GetByID(Id);
        }


        public bool UpdateUserStory(UserStory US, int Id)
        {
            UserStory DbUs = GetUserStoryById(US.Id);

            // each user will update just the needed information that he's interested in it!

            if (Id == (int)RoleEnum.ProductOwner)
            {
                DbUs.Title = US.Title;
                DbUs.Description = US.Description;
                DbUs.ProductBackLogId = US.ProductBackLogId;
                DbUs.ReleaseId = US.ReleaseId;
                DbUs.SprintId = US.SprintId;
            }

            else if (Id == (int)RoleEnum.ScrumMaster)
            {
                DbUs.SprintId = US.SprintId;
                DbUs.DevelopedId = US.DevelopedId;
                DbUs.TesterId = US.TesterId;
            }

            else if (Id == (int)RoleEnum.Developer)
            {
                if (DbUs.DevActual != US.DevActual)
                {
                    UserStoryLog USLog = new UserStoryLog();
                    USLog.ChangedByUserId = Id;
                    USLog.LogType = LogType.DevActual;
                    USLog.UserStoryId = DbUs.Id;
                    USLog.NewValue = (US.DevActual.HasValue ? US.DevActual.Value : 0) -
                                                    (DbUs.DevActual.HasValue ? DbUs.DevActual.Value : 0);

                    USLog.OldValue = DbUs.DevActual.HasValue ? DbUs.DevActual.Value : 0;
                    USLog.Date = DateTime.Now;
                    UnitOfWork.UserStoryLogRepository.Insert(USLog);
                }

                DbUs.DevEstimate = US.DevEstimate;
                DbUs.DevActual = US.DevActual;
            }

            else if (Id == (int)RoleEnum.Tester)
            {
                if (DbUs.TestActual != US.TestActual)
                {
                    UserStoryLog USLog = new UserStoryLog();
                    USLog.ChangedByUserId = Id;
                    USLog.LogType = LogType.TestActual;
                    USLog.UserStoryId = DbUs.Id;
                    USLog.NewValue = (US.TestActual.HasValue ? US.TestActual.Value : 0) -
                                                    (DbUs.TestActual.HasValue ? DbUs.TestActual.Value : 0);

                    USLog.OldValue = DbUs.TestActual.HasValue ? DbUs.TestActual.Value : 0;
                    USLog.Date = DateTime.Now;
                    UnitOfWork.UserStoryLogRepository.Insert(USLog);
                }

                DbUs.TestEstiamte = US.TestEstiamte;
                DbUs.TestActual = US.TestActual;
            }

            UnitOfWork.UserStoryRepository.Update(DbUs);
            return UnitOfWork.SaveChanges();
        }

        public void DeleteRelatedLogs(int UserStoryId)
        {
            var usLogs = UnitOfWork.UserStoryLogRepository.Get(i => i.UserStoryId == UserStoryId);
            foreach ( var log in usLogs)
            {
                UnitOfWork.UserStoryLogRepository.Delete(log.Id);
            }            
            UnitOfWork.SaveChanges();
        }
    }
}
