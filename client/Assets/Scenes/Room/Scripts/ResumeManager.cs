using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Collections.Generic;

public class ResumeManager : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_RoomObject;
	[SerializeField]
	private GameObject m_MaJiangObject;
	[SerializeField]
	private MaJiangManager m_MaJiangManager;
	[SerializeField]
	private PlayersManagerBehavior m_PlayerManager;
	[SerializeField]
	private PaiFactory m_PaiFactory;

	void Awake()
	{
		MaJiangResumeResponseParameter response = PlayerInformation.Instance.ResumeResponse;
		if(response != null)
		{
			this.m_MaJiangObject.SetActive(true);
			List<int> allPengPais = new List<int>();

			foreach (var player in response.Players) 
			{
				foreach(var pp in player.PengPai)
				{
					allPengPais.AddRange(pp);
				}
			}

			foreach (var player in response.Players) 
			{
				this.m_PlayerManager.GeneratePlayer(player.PlayerId, player.Position, true);

				if(player.PlayerId == PlayerInformation.Instance.PlayerID)
				{
					this.m_PaiFactory.CreateSelfShouPai(player.ShouPai.ToArray());

					foreach(var pp in player.PengPai)
					{
						Dictionary<int, GameObject> pengPais = this.m_PaiFactory.ConstructPengPai(0,  pp, false);
						if(response.CurrentQiangGangPai.HasValue && pp.Contains(response.CurrentQiangGangPai.Value))
						{
							this.m_PaiFactory.CurrentTempGangPai = pengPais[response.CurrentQiangGangPai.Value];
						}
					}
					foreach (var cp in player.ChuPai) 
					{
						if(!allPengPais.Contains(cp))
						{
							GameObject go = this.m_PaiFactory.ConstructChuPai(0, cp);
							if(response.CurrentChuPai.HasValue && response.CurrentChuPai.Value == cp)
							{
								this.m_PaiFactory.CurrentChuPai = go;
							}
						}
					}
					if(player.HuPai.HasValue)
					{
						this.m_PaiFactory.ConstructHuPai(0, player.HuPai.Value, false);
					}
				}
				else
				{
					int shouPaiCount = player.PlayerId.Equals(response.ActivePlayerId) && !response.CurrentMoPai.HasValue ? 14 : 13;
					shouPaiCount -= player.PengPai.Count * 3;
					int id = this.GetIndexFromPosition(player.Position);

					this.m_PaiFactory.CreateOtherShouPai(id, shouPaiCount);
					foreach(var pp in player.PengPai)
					{
						Dictionary<int, GameObject> pengPais = this.m_PaiFactory.ConstructPengPai(id,  pp, false);
						if(response.CurrentQiangGangPai.HasValue && pp.Contains(response.CurrentQiangGangPai.Value))
						{
							this.m_PaiFactory.CurrentTempGangPai = pengPais[response.CurrentQiangGangPai.Value];
						}
					}
					foreach (var cp in player.ChuPai) 
					{
						if(!allPengPais.Contains(cp))
						{
							GameObject go = this.m_PaiFactory.ConstructChuPai(id, cp);
							if(response.CurrentChuPai.HasValue && response.CurrentChuPai.Value == cp)
							{
								this.m_PaiFactory.CurrentChuPai = go;
							}
						}
					}
					if(response.CurrentMoPai.HasValue && player.PlayerId.Equals(response.ActivePlayerId))
					{
						this.m_PaiFactory.CreateOtherMoPai(id);
					}
					if(player.HuPai.HasValue)
					{
						this.m_PaiFactory.ConstructHuPai(id, player.HuPai.Value, false);
					}

					this.m_MaJiangManager.InitialPlayerPositionDict();
				}
			}
			this.m_RoomObject.SetActive(false);
            m_PaiFactory.CreateAllDiPais(response.RemainingPaiCount);
            
		}
	}

	private int GetIndexFromPosition(int position)
	{
		int result = position - PlayerInformation.Instance.RoomPosition;
		return result < 0 ? result + 4 : result;
	}
}
