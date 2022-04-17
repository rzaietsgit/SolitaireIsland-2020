using com.F4A.MobileThird;
using System;
using UnityEngine.Events;
using ITSoft;

namespace Nightingale.Ads
{
	public class AdmobVideoAd : BaseVideoAd
	{
		private bool completed;

		public override void Initialization(ThirdPartyAdData thirdPartyAdData, UnityAction<bool> unityAction)
		{
			base.Initialization(thirdPartyAdData, unityAction);
            AdsManager.OnCompleteRewardVideo += AdsManager_OnRewardedAdCompleted;
		}

        private void AdsManager_OnRewardedAdCompleted()
        {
            completed = true;
            unityAction?.Invoke(completed);
        }

        public override void Dispose()
		{
            AdsManager.OnCompleteRewardVideo -= AdsManager_OnRewardedAdCompleted;
        }

        public override bool IsReady()
		{
            return AdsManager.RewardIsReady();
		}

		public override void Show()
		{
            AdsManager.ShowRewarded();
		}
	}
}
