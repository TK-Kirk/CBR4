using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.Enums
{
    public enum CoregPartnerType
    {
        ProvideMedia = 1,
        EngageIQ = 2,
        Centerfield = 3
    }


    
    
    //use this to make enums
    //SELECT cp.Name + '_' + cc.Name + '=' + CAST(cc.CoregCampaignId AS VARCHAR) + ','  FROM dbo.CoregPartner AS CP 
    //INNER JOIN dbo.CoregCampaign AS CC ON CC.CoregPartnerId = CP.CoregPartnerId

    public enum CoregCampaignType
    {
        ProvideMedia_Debtcom = 1,
        ProvideMedia_DirectEnergy = 2,
        EngageIQ_Taxotere = 3,
        EngageIQ_RailroadCancer = 4,
        EngageIQ_HernaMesh = 5,
        EngageIQ_Xarelto = 6,
        EngageIQ_BackBrace = 7,
        EngageIQ_MedicalAlert = 8,
        EngageIQ_PainGel = 9,
        EngageIQ_Toluna = 10,
        EngageIQ_MotorVehicleAccident = 11,
        Centerfield_Sprint = 12,
        EngageIQ_MySurvey = 13,
        EngageIQ_GlobalTestMarket = 14

    }


}
