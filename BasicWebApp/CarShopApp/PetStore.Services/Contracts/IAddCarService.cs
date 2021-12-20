namespace PetStore.Services.Contracts
{
    public interface IAddCarService
    {
        void Add(string carMake, string carModel, string carYear, string carPictureURL);
    }
}
