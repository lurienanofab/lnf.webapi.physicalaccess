using LNF.PhysicalAccess;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace LNF.WebApi.PhysicalAccess.Controllers
{
    /// <summary/>
    public class DefaultController : ApiController
    {
        /// <summary/>
        public IProvider Provider { get; }

        /// <summary/>
        public DefaultController(IProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Gets the API name.
        /// </summary>
        [AllowAnonymous, Route("")]
        public string Get()
        {
            return "physical-access-api";
        }

        /// <summary>
        /// Gets a list of all badges. There is one badge per client.
        /// </summary>
        /// <param name="clientId">(optional) Filters results to a single client when greater than zero.</param>
        [Route("badge/{clientId?}")]
        public IEnumerable<Badge> GetBadge(int clientId = 0)
        {
            return Provider.PhysicalAccess.GetBadge(clientId);
        }

        /// <summary>
        /// Gets a list of all cards. A badge can have multiple cards.
        /// </summary>
        /// <param name="clientId">(optional) Filters results to a single client when greater than zero.</param>
        [Route("cards/{clientId?}")]
        public IEnumerable<Card> GetCards(int clientId = 0)
        {
            return Provider.PhysicalAccess.GetCards(clientId);
        }

        /// <summary>
        /// Gets clients whose cards will expire before the cutoff date.
        /// </summary>
        /// <param name="cutoff">The cutoff date.</param>
        [Route("cards/expiring")]
        public IEnumerable<Card> GetExpiringCards(DateTime cutoff)
        {
            return Provider.PhysicalAccess.GetExpiringCards(cutoff);
        }

        /// <summary>
        /// Gets a list of lab areas.
        /// </summary>
        [Route("areas")]
        public IEnumerable<Area> GetAreas()
        {
            return Provider.PhysicalAccess.GetAreas();
        }

        /// <summary>
        /// Gets a list of badges that are currently lab area occupants.
        /// </summary>
        /// <param name="alias">The room alias: all|cleanroom|robin</param>
        [Route("area/{alias?}")]
        public IEnumerable<Badge> GetCurrentlyInArea(string alias = "all")
        {
            return Provider.PhysicalAccess.GetCurrentlyInArea(alias);
        }


        /// <summary>
        /// Gets a list of lab areas and the occupants in each area.
        /// </summary>
        /// <param name="alias">The room alias: all|cleanroom|robin</param>
        [Route("badge-in-area/{alias?}")]
        public IEnumerable<BadgeInArea> GetBadgeInAreas(string alias = "all")
        {
            return Provider.PhysicalAccess.GetBadgeInAreas(alias);
        }

        /// <summary>
        /// Gets a list of events (e.g. entering or leaving an area). Data is prepared for importing.
        /// </summary>
        /// <param name="sd">The start date.</param>
        /// <param name="ed">The end date.</param>
        /// <param name="clientId">The ClientID for a specific client, or zero for all.</param>
        /// <param name="roomId">The RoomID for a specific room, or zero for all.</param>
        [Route("events")]
        public IEnumerable<Event> GetEvents(DateTime sd, DateTime ed, int clientId = 0, int roomId = 0)
        {
            return Provider.PhysicalAccess.GetEvents(sd, ed, clientId, roomId);
        }

        /// <summary>
        /// Gets a list of events (e.g. entering or leaving an area). No data preparation takes place.
        /// </summary>
        /// <param name="sd">The start date.</param>
        /// <param name="ed">The end date.</param>
        /// <param name="clientId">The ClientID for a specific client, or zero for all.</param>
        /// <param name="roomId">The RoomID for a specific room, or zero for all.</param>
        /// <returns></returns>
        [Route("events/raw")]
        public IEnumerable<Event> GetRawData(DateTime sd, DateTime ed, int clientId = 0, int roomId = 0)
        {
            return Provider.PhysicalAccess.GetRawData(sd, ed, clientId, roomId);
        }

        /// <summary>
        /// Finds the previous IN event before the given event.
        /// </summary>
        /// <param name="request">The requested EventID and StartDate. Events before StartDate are not included.</param>
        [HttpPost, Route("events/find-previous-in")]
        public Event FindPreviousIn([FromBody] FindPreviousInRequest request)
        {
            return Provider.PhysicalAccess.FindPreviousIn(request);
        }

        /// <summary>
        /// Finds the next OUT event after the given event.
        /// </summary>
        /// <param name="request">The requested EventID and EndDate. Events after EndDate are not included.</param>
        [HttpPost, Route("events/find-next-out")]
        public Event FindNextOut([FromBody] FindNextOutRequest request)
        {
            return Provider.PhysicalAccess.FindNextOut(request);
        }

        /// <summary>
        /// Checks if a client can be reenabled.
        /// </summary>
        /// <param name="clientId">The ClientID.</param>
        /// <param name="days">The number of days after which an expired client cannot be reenabled.</param>
        [Route("allow-reenable")]
        public bool GetAllowReenable(int clientId, int days)
        {
            return Provider.PhysicalAccess.GetAllowReenable(clientId, days);
        }

        /// <summary>
        /// Gets a ClientID array of those clients who had passback violations during the period.
        /// </summary>
        /// <param name="sd">The start date.</param>
        /// <param name="ed">The end date.</param>
        [Route("passback-violations")]
        public IEnumerable<int> GetPassbackViolations(DateTime sd, DateTime ed)
        {
            return Provider.PhysicalAccess.GetPassbackViolations(sd, ed);
        }

        /// <summary>
        /// Adds a client to the physical access system.
        /// </summary>
        /// <param name="request">The client data to add.</param>
        [HttpPost, Route("client/add")]
        public int AddClient([FromBody] AddClientRequest request)
        {
            return Provider.PhysicalAccess.AddClient(request);
        }

        /// <summary>
        /// Enables access for a client.
        /// </summary>
        /// <param name="request">The client id and optional expiration date.</param>
        [HttpPost, Route("client/enable")]
        public int EnableAccess([FromBody] UpdateClientRequest request)
        {
            return Provider.PhysicalAccess.EnableAccess(request);
        }

        /// <summary>
        /// Disables access for a client.
        /// </summary>
        /// <param name="request">The client id and optional expiration date.</param>
        [HttpPost, Route("client/disable")]
        public int DisableAccess([FromBody] UpdateClientRequest request)
        {
            return Provider.PhysicalAccess.DisableAccess(request);
        }
    }
}
