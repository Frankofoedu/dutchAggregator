using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Bet9jaData
{
    public class Bet9jaReceivedData
    {
        public List<D> d { get; set; }
    }
    public class TipiQuotaGruppo
    {
        public int IDSport { get; set; }
        public int IDTipoQuota { get; set; }
        public string TipoQuota { get; set; }
        public string TipoQuotaBreve { get; set; }
        public int Riga { get; set; }
        public int Colonna { get; set; }
        public int IDClasseQuota { get; set; }
    }

    public class ClassiQuotaList
    {
        public int IDSport { get; set; }
        public string CodiceClasseQuota { get; set; }
        public string ClasseQuota { get; set; }
        public string ClasseQuotaBreve { get; set; }
        public string DescrizioneCompleta { get; set; }
        public int IDClasseQuota { get; set; }
        public int IDGruppoQuota { get; set; }
        public double HND { get; set; }
        public int TipoHND { get; set; }
        public List<TipiQuotaGruppo> TipiQuotaGruppo { get; set; }
    }

    public class Quote
    {
        public int IDSport { get; set; }
        public int IDEvento { get; set; }
        public int IDSottoEvento { get; set; }
        public object IDQuota { get; set; }
        public double QuotaValore { get; set; }
        public double HND { get; set; }
        public int TipoHND { get; set; }
        public string StrHND { get; set; }
        public int IDTipoQuota { get; set; }
        public string TipoQuota { get; set; }
        public string TipoQuotaBreve { get; set; }
        public int IDClasseQuota { get; set; }
        public int Giocabilita { get; set; }
        public int IDTipoSport { get; set; }
        public int Riga { get; set; }
        public int Colonna { get; set; }
        public DateTime DataInizio { get; set; }
        public string StrQuotaValore { get; set; }
    }

    public class SottoEventiList
    {
        public int IDSport { get; set; }
        public int IDEvento { get; set; }
        public string Evento { get; set; }
        public int IDSottoEvento { get; set; }
        public string SottoEvento { get; set; }
        public DateTime DataInizio { get; set; }
        public int CodPubblicazione { get; set; }
        public int Stato { get; set; }
        public int Ordine { get; set; }
        public int QuoteCount { get; set; }
        public int ClassiQuotaCount { get; set; }
        public List<Quote> Quote { get; set; }
        public string CodiceStatistiche { get; set; }
        public string MarketMoversAbilitato { get; set; }
        public int Allibraggio { get; set; }
        public string ImporterCode { get; set; }
        public string StrDataInizio { get; set; }
        public string StrDataInizioBreve { get; set; }
        public string StrOraInizio { get; set; }
        public string SottoEventoDesc { get; set; }
    }

    public class Detail
    {
        public int IDBookmaker { get; set; }
        public int IDSport { get; set; }
        public int IDGruppoQuota { get; set; }
        public object AbilitazioniBetradar { get; set; }
        public List<ClassiQuotaList> ClassiQuotaList { get; set; }
        public List<SottoEventiList> SottoEventiList { get; set; }
    }

    public class D
    {
        public string __type { get; set; }
        public int IDSport { get; set; }
        public int IDTipoSport { get; set; }
        public string Sport { get; set; }
        public int QuoteCount { get; set; }
        public bool Attivo { get; set; }
        public Detail Detail { get; set; }
        public List<object> CategorieList { get; set; }
    }

}
