using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public enum ExchangeSegment
    {
        /// <summary>
        /// NSE cash
        /// </summary>
        NSECM = 1,
        /// <summary>
        /// NSE FnO
        /// </summary>
        NSEFO = 2,
        /// <summary>
        /// NSE CDS
        /// </summary>
        NSECD = 3,
        /// <summary>
        /// BSE Cash
        /// </summary>
        BSECM = 11,
        /// <summary>
        /// BSE FnO
        /// </summary>
        BSEFO = 12,
        /// <summary>
        /// BSE CDS
        /// </summary>
        BSECD = 13,
        /// <summary>
        /// NCDEX
        /// </summary>
        NCDEX = 21,
        /// <summary>
        /// MSEI Cash
        /// </summary>
        MSEICM = 41,
        /// <summary>
        /// MSEI FnO
        /// </summary>
        MSEIFO = 42,
        /// <summary>
        /// MSEI CDS
        /// </summary>
        MSEICD = 43,
        /// <summary>
        /// MCX FnO
        /// </summary>
        MCXFO = 51
    }
    public enum MarketDataPorts
    {
        /// <summary>
        /// Level I vents
        /// </summary>
        touchlineEvent = 1501,
        /// <summary>
        /// Level II events
        /// </summary>
        marketDepthEvent = 1502,
        /// <summary>
        /// Top gainers and loser event
        /// </summary>
        topGainerLosserEvent = 1503,
        /// <summary>
        /// Index event
        /// </summary>
        indexDataEvent = 1504,
        /// <summary>
        /// Candle event
        /// </summary>
        candleDataEvent = 1505,
        /// <summary>
        /// General message broadcast event
        /// </summary>
        generalMessageBroadcastEvent = 1506,
        /// <summary>
        /// Exchange trading status event
        /// </summary>
        exchangeTradingStatusEvent = 1507,
        /// <summary>
        /// Open interest event
        /// </summary>
        openInterestEvent = 1510,
        /// <summary>
        /// Instrument subscription info
        /// </summary>
        instrumentSubscriptionInfo = 5505,
        /// <summary>
        /// Level III 30 depth picture
        /// </summary>
        marketDepthEvent30 = 5014,
        /// <summary>
        /// Level III 100 depth picture
        /// </summary>
        marketDepthEvent100 = 5018
    }
}
