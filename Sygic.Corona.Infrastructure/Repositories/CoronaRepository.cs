using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Infrastructure.Repositories
{
    public class CoronaRepository : IRepository
    {
        private readonly CoronaContext context;
        public IUnitOfWork UnitOfWork => context;

        public CoronaRepository(CoronaContext context)
        {
            this.context = context;
        }

        public async Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken)
        {
            await context.Profiles.AddAsync(profile, cancellationToken);
        }

        public async Task CreateContactAsync(Contact contact, CancellationToken cancellationToken)
        {
            await context.Contacts.AddAsync(contact, cancellationToken);
        }

        public async Task CreateLocationAsync(Location location, CancellationToken cancellationToken)
        {
            await context.Locations.AddAsync(location, cancellationToken);
        }

        public Task<Profile> GetProfileAsync(long profileId, string deviceId, CancellationToken cancellationToken)
        {
            return context.Profiles.SingleOrDefaultAsync(x => x.Id == profileId && x.DeviceId == deviceId,
                cancellationToken);
        }

        public Task<Profile> GetProfileAsync(long profileId, string deviceId, string covidPass, CancellationToken cancellationToken)
        {
            return context.Profiles.SingleOrDefaultAsync(x => x.Id == profileId && x.DeviceId == deviceId && x.CovidPass == covidPass,
                cancellationToken);
        }

        public Task<Profile> GetProfileAsyncNt(long profileId, string deviceId, CancellationToken cancellationToken)
        {
            return context.Profiles
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == profileId && x.DeviceId == deviceId, cancellationToken);
        }
        
        public Task<Profile> GetProfileAsync(string deviceId, CancellationToken cancellationToken)
        {
            return context.Profiles.SingleOrDefaultAsync(x => x.DeviceId == deviceId, cancellationToken);
        }

        public async Task<IEnumerable<Profile>> GetProfilesByCovidPassAsync(string covidPass, CancellationToken cancellationToken)
        {
            return await context.Profiles.Where(x => x.CovidPass == covidPass)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AlreadyCreatedAsync(string deviceId, CancellationToken cancellationToken)
        {
            var profile = await context.Profiles.FirstOrDefaultAsync(x => x.DeviceId == deviceId, cancellationToken);

            return profile != null;
        }

        public async Task<string> GetProfilePushTokenAsync(long profileId, string deviceId, CancellationToken cancellationToken)
        {
            var result = await context.Profiles.Select(x => new {x.Id, x.DeviceId, x.PushToken})
                .SingleOrDefaultAsync(x => x.Id == profileId && x.DeviceId == deviceId, cancellationToken);
            return result.PushToken;
        }

        public async Task<string> GetProfilePushTokenAsync(long profileId, CancellationToken cancellationToken)
        {
            var result = await context.Profiles.Where(x => x.Id == profileId)
                .Select(x => new { x.PushToken })
                .FirstOrDefaultAsync(cancellationToken);

            return result.PushToken;
        }

        public async Task<string> GetProfileMfaTokenAsync(long profileId, string deviceId, CancellationToken cancellationToken)
        {
            var result = await context.Profiles.Where(x => x.Id == profileId && x.DeviceId == deviceId)
                .Select(x => new { x.AuthToken })
                .FirstOrDefaultAsync(cancellationToken);

            return result.AuthToken;
        }

        public async Task<bool> GetProfileInfectionStatusAsync(long profileId, string deviceId, CancellationToken cancellationToken)
        {
            bool status = await context.Profiles.Where(x => x.Id == profileId && x.DeviceId == deviceId)
                .Select(x => x.ConfirmedInfection).SingleOrDefaultAsync(cancellationToken);

            return status;
        }

        public async Task<IEnumerable<Contact>> GetContactsForProfileAsync(long profileId, CancellationToken cancellationToken)
        {
            return await context.Contacts.Where(x => x.SeenProfileId == profileId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Contact>> GetContactsForProfileAsyncNt(long profileId, CancellationToken ct)
        {
            return await context.Contacts
                .AsNoTracking()
                .Where(x => x.SeenProfileId == profileId)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Alert>> GetAlertsForProfileAsyncNt(long profileId, string deviceId, CancellationToken cancellationToken)
        {
            return await context.Alerts
                .AsNoTracking()
                .Where(x => x.ProfileId == profileId && x.DeviceId == deviceId)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(cancellationToken);
        }

        public IQueryable<Alert> GetAlertsForProfileNt(long profileId, string deviceId)
        {
            return context.Alerts
                .AsNoTracking()
                .Where(x => x.ProfileId == profileId && x.DeviceId == deviceId)
                .OrderByDescending(x => x.CreatedOn)
                .AsQueryable();
        }

        public async Task<IEnumerable<Location>> GetLocationsForProfileNt(long profileId, CancellationToken ct)
        {
            var locations = await context.Locations
                .AsNoTracking()
                .Where(x => x.ProfileId == profileId)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(ct);

            return locations;
        }

        public async Task<Location> GetLastLocationForProfileNt(long profileId, CancellationToken ct)
        {
            var locations = await context.Locations
                .AsNoTracking()
                .Where(x => x.ProfileId == profileId)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(ct);

            return locations;
        }

        /// <summary>
        /// Delete all records older than timestamp
        /// </summary>
        /// <param name="interval">epoch timestamp</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteContactsAsync(DateTime interval, CancellationToken cancellationToken)
        {
            var contacts = await context.Contacts
                .Where(x => x.CreatedOn < interval)
                .ToListAsync(cancellationToken);

            context.Contacts.RemoveRange(contacts);
        }

        public async Task DeleteLocationsAsync(DateTime interval, CancellationToken cancellationToken)
        {
            var locations = await context.Locations
                .Where(x => x.CreatedOn < interval)
                .ToListAsync(cancellationToken);

            context.Locations.RemoveRange(locations);
        }

        //TODO
        public async Task<IEnumerable<GetQuarantineListResponse>> GetProfilesInQuarantineAsync(DateTime? from, CancellationToken cancellationToken)
        {
            return new List<GetQuarantineListResponse>();
            //var now = DateTime.UtcNow;
            //var profiles = await context.Profiles
            //    .AsNoTracking()
            //    .Where(x => x.IsInQuarantine && x.QuarantineEnd > now && x.QuarantineBeginning >= from)
            //    .Select(x => new {x.Id, x.CovidPass, x.CreatedOn, x.LastPositionReportTime, x.QuarantineBeginning, x.QuarantineEnd, x.AreaExit})
            //    .ToListAsync(cancellationToken);

            //var response = profiles.Select(x => new GetQuarantineListResponse
            //{
            //    Id = (uint)x.Id,
            //    CovidPass = x.CovidPass,
            //    CreatedOn = x.CreatedOn,
            //    Quarantine = new QuarantineResponse
            //    {
            //        QuarantineBeginning = x.QuarantineBeginning,
            //        QuarantineEnd = x.QuarantineEnd,
            //        QuarantineExit = x.AreaExit != null ? new QuarantineExitResponse
            //        {
            //            Accuracy = x.AreaExit?.Accuracy,
            //            Latitude = x.AreaExit?.Latitude,
            //            Longitude = x.AreaExit?.Longitude,
            //            AreaExitTime = x.AreaExit?.RecordDateUtc
            //        } : null
            //    },
            //    LastLocation = x.LastPositionReportTime != null ? new LastLocationResponse
            //    {
            //        LastReportedTime = x.LastPositionReportTime
            //    } : null
            //}).ToDictionary(x => x.Id);

            //var ids = response.Keys;
            //foreach (uint id in ids)
            //{
            //    var lastLocation = await context.Locations.Where(x => x.ProfileId == id)
            //        .Select(x => new {x.Latitude, x.Longitude, x.Accuracy, x.CreatedOn})
            //        .OrderByDescending(x => x.CreatedOn)
            //        .FirstOrDefaultAsync(cancellationToken);
            //    if (lastLocation != null)
            //    {
            //        var profile = response[id];
            //        if (profile != null)
            //        {
            //            if (profile.LastLocation != null)
            //            {
            //                profile.LastLocation.Latitude = lastLocation.Latitude;
            //                profile.LastLocation.Longitude = lastLocation.Longitude;
            //                profile.LastLocation.Accuracy = lastLocation.Accuracy;
            //            }
            //        }
            //    }
            //}
            
            //return response.Select(x => x.Value);
        }

        public async Task<IEnumerable<Profile>> GetRawProfilesInQuarantineAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            return await context.Profiles
                .AsNoTracking()
                .Where(x => x.QuarantineEnd >= now)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Profile>> GetInactiveUsersInQuarantineAsync(DateTime from, CancellationToken cancellationToken)
        {
            var result = new List<Profile>();
            var now = DateTime.UtcNow;

            var inactiveProfileCandidates = await context.Profiles
                .Where(x => (x.LastPositionReportTime < from || !x.LastPositionReportTime.HasValue) && x.QuarantineEnd > now)
                .ToListAsync(cancellationToken);

            var inactiveProfileCandidatesGroup = inactiveProfileCandidates
                .OrderByDescending(x => x.QuarantineEnd)
                .GroupBy(x => x.CovidPass)
                .ToList();

            //Find if other profile which are sending position with same phone number exist.
            //TODO maybe it is better to add creation date to profile and always use latest one ?
            var phoneNumbers = inactiveProfileCandidatesGroup.Select(p => p.Key);
            var otherActiveProfilesWithSamePhoneNumber = await context.Profiles
                .Where(x => phoneNumbers.Contains(x.CovidPass) && x.LastPositionReportTime >= from && x.QuarantineEnd > now)
                .Select(x => x.CovidPass)
                .ToListAsync(cancellationToken);

            inactiveProfileCandidatesGroup.RemoveAll(x => otherActiveProfilesWithSamePhoneNumber.Contains(x.Key));

            foreach (var inactiveProfileCandidateList in inactiveProfileCandidatesGroup)
            {
                var lastInactiveProfile = inactiveProfileCandidateList
                    .Select(x => x)
                    .OrderByDescending(x => x.QuarantineEnd)
                    .FirstOrDefault();
                result.Add(lastInactiveProfile);
            }

            return result;
        }
    }
}
