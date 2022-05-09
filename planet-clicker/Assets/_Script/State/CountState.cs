using Bencodex.Types;
 using Libplanet.Store;

 namespace _Script.State
 {
     public class CountState : DataModel
     {
         public long count { get; private set; }

         public CountState(long c)
         {
             count = c;
         }

         public CountState(Bencodex.Types.Dictionary encoded)
             : base(encoded)
         {
         }
     }
 }