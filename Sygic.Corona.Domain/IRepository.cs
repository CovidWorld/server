using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Domain
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken);
        Task CreateContactAsync(Contact contact, CancellationToken cancellationToken);
        Task CreateLocationAsync(Location location, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsync(uint profileId, string deviceId, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsyncNt(uint profileId, string deviceId, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsync(string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<Profile>> GetProfilesByCovidPassAsync(string covidPass, CancellationToken cancellationToken);
        Task<uint> GetLastIdAsync(CancellationToken cancellationToken);
        Task<bool> AlreadyCreatedAsync(string deviceId, CancellationToken cancellationToken);
        Task<string> GetProfilePushTokenAsync(uint profileId, string deviceId, CancellationToken cancellationToken);
        Task<string> GetProfilePushTokenAsync(uint profileId, CancellationToken cancellationToken);
        Task<string> GetProfileMfaTokenAsync(uint profileId, string deviceId, CancellationToken cancellationToken);
        Task<bool> GetProfileInfectionStatusAsync(uint profileId, string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> GetContactsForProfileAsync(uint profileId, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> GetContactsForProfileAsyncNt(uint profileId, CancellationToken cancellationToken);
        Task<IEnumerable<Alert>> GetAlertsForProfileAsyncNt(uint profileId, string deviceId, CancellationToken cancellationToken);
        IQueryable<Alert> GetAlertsForProfileNt(uint profileId, string deviceId);
        Task<IEnumerable<GetQuarantineListResponse>> GetProfilesInQuarantineAsync(DateTime? from, CancellationToken cancellationToken);
        Task<IEnumerable<Profile>> GetRawProfilesInQuarantineAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Profile>> GetInactiveUsersInQuarantineAsync(DateTime from, CancellationToken cancellationToken);
        Task<IEnumerable<Location>> GetLocationsForProfileNt(uint profileId, CancellationToken ct);
        Task<Location> GetLastLocationForProfileNt(uint profileId, CancellationToken ct);
        Task DeleteContactsAsync(int interval, CancellationToken cancellationToken);
        Task DeleteLocationsAsync(DateTime interval, CancellationToken cancellationToken);
    }
}
