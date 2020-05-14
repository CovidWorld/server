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
        Task<Profile> GetProfileAsync(long profileId, string deviceId, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsync(long profileId, string deviceId, string covidPass, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsyncNt(long profileId, string deviceId, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsync(string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<Profile>> GetProfilesByCovidPassAsync(string covidPass, CancellationToken cancellationToken);
        Task<bool> AlreadyCreatedAsync(string deviceId, CancellationToken cancellationToken);
        Task<string> GetProfilePushTokenAsync(long profileId, string deviceId, CancellationToken cancellationToken);
        Task<string> GetProfilePushTokenAsync(long profileId, CancellationToken cancellationToken);
        Task<string> GetProfileMfaTokenAsync(long profileId, string deviceId, CancellationToken cancellationToken);
        Task<bool> GetProfileInfectionStatusAsync(long profileId, string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> GetContactsForProfileAsync(long profileId, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> GetContactsForProfileAsyncNt(long profileId, CancellationToken cancellationToken);
        Task<IEnumerable<Alert>> GetAlertsForProfileAsyncNt(long profileId, string deviceId, CancellationToken cancellationToken);
        IQueryable<Alert> GetAlertsForProfileNt(long profileId, string deviceId);
        Task<IEnumerable<GetQuarantineListResponse>> GetProfilesInQuarantineAsync(DateTime? from, CancellationToken cancellationToken);
        Task<IEnumerable<Profile>> GetRawProfilesInQuarantineAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Profile>> GetInactiveUsersInQuarantineAsync(DateTime from, CancellationToken cancellationToken);
        Task<IEnumerable<Location>> GetLocationsForProfileNt(long profileId, CancellationToken ct);
        Task<Location> GetLastLocationForProfileNt(long profileId, CancellationToken ct);
        Task DeleteContactsAsync(DateTime interval, CancellationToken cancellationToken);
        Task DeleteLocationsAsync(DateTime interval, CancellationToken cancellationToken);
    }
}
