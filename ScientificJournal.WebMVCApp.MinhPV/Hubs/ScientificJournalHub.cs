using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.WebMVCApp.MinhPV.Hubs
{
    public class ScientificJournalHub : Hub
    {
        private readonly IJournalsMinhPvService _journalsService;

        public ScientificJournalHub(IJournalsMinhPvService journalsService)
        {
            _journalsService = journalsService;
        }

        /// <summary>
        /// Create a journal from the SignalR client and notify all connected journal index pages.
        /// </summary>
        public async Task HubCreateJournal(string journalJson)
        {
            var journal = JsonSerializer.Deserialize<JournalsMinhPv>(
                journalJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (journal == null || string.IsNullOrWhiteSpace(journal.JournalName))
            {
                await Clients.Caller.SendAsync("ReceiveJournalCreateFailed", "Journal name is required.");
                return;
            }

            journal.CreatedAt = DateTime.Now;
            await _journalsService.CreateAsync(journal);

            var createdJournal = await _journalsService.GetDetailAsync(journal.JournalIdMinhPv);
            await Clients.All.SendAsync("ReceiveJournalCreated", new
            {
                journalIdMinhPv = journal.JournalIdMinhPv,
                journalName = journal.JournalName,
                publisherName = createdJournal?.Publisher?.PublisherName ?? string.Empty,
                issn = journal.Issn ?? string.Empty,
                country = journal.Country ?? string.Empty,
                impactFactor = journal.ImpactFactor,
                isActive = journal.IsActive
            });
        }

        /// <summary>
        /// Delete a journal from the SignalR client and notify all connected journal index pages.
        /// </summary>
        public async Task HubDeleteJournal(string journalId)
        {
            if (!int.TryParse(journalId, out var id))
            {
                await Clients.Caller.SendAsync("ReceiveJournalDeleteFailed", "Invalid journal id.");
                return;
            }

            try
            {
                var deleted = await _journalsService.DeleteAsync(id);
                if (deleted > 0)
                {
                    await Clients.All.SendAsync("ReceiveJournalDeleted", id);
                    return;
                }

                await Clients.Caller.SendAsync("ReceiveJournalDeleteFailed", "Journal not found.");
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveJournalDeleteFailed", "Cannot delete this journal because it has related data.");
            }
        }

        public async Task HubEditJournal(string journalJson)
        {
            var journal = JsonSerializer.Deserialize<JournalsMinhPv>(
                journalJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (journal == null || journal.JournalIdMinhPv <= 0 || string.IsNullOrWhiteSpace(journal.JournalName))
            {
                await Clients.Caller.SendAsync("ReceiveJournalUpdateFailed", "Valid journal id and name are required.");
                return;
            }
            var existingJournal = await _journalsService.GetDetailAsync(journal.JournalIdMinhPv);
            if (existingJournal == null)
            {
                await Clients.Caller.SendAsync("ReceiveJournalUpdateFailed", "Journal not found.");
                return;
            }
            existingJournal.JournalName = journal.JournalName;
            existingJournal.Issn = journal.Issn;
            existingJournal.Country = journal.Country;
            existingJournal.ImpactFactor = journal.ImpactFactor;
            existingJournal.IsActive = journal.IsActive;
            await _journalsService.UpdateAsync(existingJournal);
            var updatedJournal = await _journalsService.GetDetailAsync(journal.JournalIdMinhPv);
            await Clients.All.SendAsync("ReceiveJournalUpdated", new
            {
                journalIdMinhPv = updatedJournal?.JournalIdMinhPv,
                journalName = updatedJournal?.JournalName,
                publisherName = updatedJournal?.Publisher?.PublisherName ?? string.Empty,
                issn = updatedJournal?.Issn ?? string.Empty,
                country = updatedJournal?.Country ?? string.Empty,
                impactFactor = updatedJournal?.ImpactFactor,
                isActive = updatedJournal?.IsActive
            });
        }
    }
}
