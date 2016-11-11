﻿namespace SoundFingerprinting
{
    using System.Collections.Generic;
    using System.Linq;

    using SoundFingerprinting.Configuration;
    using SoundFingerprinting.DAO;
    using SoundFingerprinting.DAO.Data;
    using SoundFingerprinting.Data;

    public abstract class ModelService : IModelService
    {
        private readonly ITrackDao trackDao;
        private readonly ISubFingerprintDao subFingerprintDao;

        protected ModelService(ITrackDao trackDao, ISubFingerprintDao subFingerprintDao)
        {
            this.trackDao = trackDao;
            this.subFingerprintDao = subFingerprintDao;
        }
 
        public virtual IList<SubFingerprintData> ReadSubFingerprints(long[] hashBins, QueryConfiguration config)
        {
            if (!string.IsNullOrEmpty(config.TrackGroupId))
            {
                return subFingerprintDao.ReadSubFingerprints(hashBins, config.ThresholdVotes, config.TrackGroupId).ToList();
            }

            return subFingerprintDao.ReadSubFingerprints(hashBins, config.ThresholdVotes).ToList();
        }

        public virtual ISet<SubFingerprintData> ReadSubFingerprints(IEnumerable<long[]> hashes, QueryConfiguration config)
        {
            return subFingerprintDao.ReadSubFingerprints(hashes, config.ThresholdVotes);
        }

        public virtual bool ContainsTrack(string isrc, string artist, string title)
        {
            if (!string.IsNullOrEmpty(isrc))
            {
                return ReadTrackByISRC(isrc) != null;
            }

            return ReadTrackByArtistAndTitleName(artist, title).Any();
        }

        public virtual IModelReference InsertTrack(TrackData track)
        {
            return trackDao.InsertTrack(track);
        }

        public virtual void InsertHashDataForTrack(IEnumerable<HashedFingerprint> hashes, IModelReference trackReference)
        {
            subFingerprintDao.InsertHashDataForTrack(hashes, trackReference);
        }

        public virtual IList<HashedFingerprint> ReadHashedFingerprintsByTrack(IModelReference trackReference)
        {
            return subFingerprintDao.ReadHashedFingerprintsByTrackReference(trackReference);
        }

        public virtual IList<TrackData> ReadAllTracks()
        {
            return trackDao.ReadAll();
        }

        public virtual IList<TrackData> ReadTrackByArtistAndTitleName(string artist, string title)
        {
            return trackDao.ReadTrackByArtistAndTitleName(artist, title);
        }

        public virtual TrackData ReadTrackByReference(IModelReference trackReference)
        {
            return trackDao.ReadTrack(trackReference);
        }

        public virtual TrackData ReadTrackByISRC(string isrc)
        {
            return trackDao.ReadTrackByISRC(isrc);
        }

        public virtual int DeleteTrack(IModelReference trackReference)
        {
            return trackDao.DeleteTrack(trackReference);
        }
    }
}
