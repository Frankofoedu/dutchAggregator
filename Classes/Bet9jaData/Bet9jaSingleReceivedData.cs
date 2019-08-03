using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Bet9jaData.Single
{
    public class Bet9jaSingleReceivedData
    {
        public D d { get; set; }
    }
    public class GruppiQuotaList
    {
        public int IDCategoriaGruppoQuota { get; set; }
        public int IDGruppoQuota { get; set; }
        public string CodiceGruppoQuota { get; set; }
        public string GruppoQuota { get; set; }
        public string GruppoQuotaBreve { get; set; }
        public int Righe { get; set; }
        public int Colonne { get; set; }
        public int Ordine { get; set; }
        public int TipoHND { get; set; }
    }

    public class CategorieList
    {
        public int IDCategoriaGruppoQuota { get; set; }
        public string NomeCategoriaGruppoQuota { get; set; }
        public string CodiceCategoria { get; set; }
        public object Descrizione { get; set; }
        public string Note { get; set; }
        public int Ordine { get; set; }
        public List<GruppiQuotaList> GruppiQuotaList { get; set; }
    }

    public class QuoteList
    {
        public int IDClasseQuota { get; set; }
        public int IDTipoQuota { get; set; }
        public object IDQuota { get; set; }
        public double? QuotaValore { get; set; }
        public string Quota { get; set; }
        public double? hnd { get; set; }
        public int SegnoHandicap { get; set; }
        public int TipoHND { get; set; }
        public int? Giocabilita { get; set; }
        public string GiocabilitaCodice { get; set; }
        public int Riga { get; set; }
        public int Colonna { get; set; }
        public string GiocabilitaCodiceStringa { get; set; }
        public int IDClasseQuotaUsata { get; set; }
        public string TipoQuotaBreve { get; set; }
        public string TipoQuota { get; set; }
        public int Formato { get; set; }
        public bool ArrotandaQuotaPerEccesso { get; set; }
        public int MaxArrotondamentoQuota { get; set; }
        public bool QuotaSingola { get; set; }
        public int NumeroMaxCifreQuota { get; set; }
        public string Handicap { get; set; }
        public bool parBookRaggruppaSottoEventiCoupon { get; set; }
        public bool parBookVisualizzaSegnoHandicap { get; set; }
        public bool parBookVisHandicapClasseQuota { get; set; }
    }

    public class ClassiQuotaList
    {
        public List<QuoteList> QuoteList { get; set; }
        public int IDClasseQuota { get; set; }
        public string ClasseQuota { get; set; }
        public string DescrizioneBreve { get; set; }
        public string DescrizioneCompleta { get; set; }
        public int Righe { get; set; }
        public int Colonne { get; set; }
        public int Ordine { get; set; }
        public double ValoreHNDDefault { get; set; }
        public int HND { get; set; }
        public double ValoreHND { get; set; }
        public bool VisualizzaHNDDettaglioSottoEvento { get; set; }
        public int IDGruppoQuota { get; set; }
        public string Spread { get; set; }
    }

    public class D
    {
        public List<CategorieList> CategorieList { get; set; }
        public List<ClassiQuotaList> ClassiQuotaList { get; set; }
        public string Sport { get; set; }
        public string Gruppo { get; set; }
        public string Evento { get; set; }
        public int IDSottoEvento { get; set; }
        public string SottoEvento { get; set; }
        public DateTime DataInizio { get; set; }
        public string SrtDataInizio { get; set; }
        public string ImporterCode { get; set; }
        public string CodiceStatistiche { get; set; }
        public int Antepost { get; set; }
        public int IDEvento { get; set; }
        public int IDSport { get; set; }
        public int IDBookmaker { get; set; }
        public int IDGMT { get; set; }
        public string AbilitazioniWidgetBetRadar { get; set; }
    }

    
}
