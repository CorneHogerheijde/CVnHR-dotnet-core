<template>
  <div>
    <div v-if="item && !success">
      Foutmelding: {{ item.meldingen.fout[0].code }}: {{item.meldingen.fout[0].omschrijving}}
      <objectTree :item="item" />
    </div>
    <div v-if="item && success" class="container">
      <div class="row">
        <div class="col-md-4">Bedrijfsnaam:</div>
        <div class="col-md-8"><strong>{{ kvkItem.naam }}</strong></div>
      </div>
      <div class="row">
        <div class="col-md-4">Kvknummer:</div>
        <div class="col-md-8">{{ kvkItem.kvkNummer }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Peilmoment:</div>
        <div class="col-md-8">{{ [ item.peilmoment, 'YYYYMMDDHHmmssSSS'] | moment('YYYY-MM-DD HH:mm:ss') }}</div>
      </div>

      <div class="row">
        <div class="col-md-4">Datum Oprichting:</div>
        <div class="col-md-8">{{ formatDate(get('heeftAlsEigenaar.item.datumOprichting') || kvkItem.registratie.datumAanvang, 'YYYYMMDDHHmmssSSS','YYYY-MM-DD') }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Datum Uitschrijving:</div>
        <div class="col-md-8">{{ formatDate(get('heeftAlsEigenaar.item.datumUitschrijving') || kvkItem.registratie.datumEinde, 'YYYYMMDDHHmmssSSS', 'YYYY-MM-DD HH:mm:ss') }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Registratie datum:</div>
        <div class="col-md-8">{{ formatDate(kvkItem.registratie.datumAanvang, 'YYYYMMDDHHmmssSSS','YYYY-MM-DD') }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Registratie datum einde:</div>
        <div class="col-md-8">{{ formatDate(kvkItem.registratie.datumEinde, 'YYYYMMDDHHmmssSSS', 'YYYY-MM-DD HH:mm:ss') }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Handelsnamen:</div>
        <div class="offset-md-4 col-md-12" v-for="h in get('manifesteertZichAls.onderneming.handeltOnder')" :key='h.handelsnaam.naam'>
          {{ h.handelsnaam.naam }} (sinds: {{formatDate(get('handelsnaam.registratie.datumAanvang', h), null, 'YYYY-MM-DD')}})
        </div>
      </div>
      <div class="row">
        <div class="col-md-4">Berichtenbox:</div>
        <div class="col-md-8">{{ kvkItem.berichtenbox }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Nonmailing:</div>
        <div class="col-md-8">{{ get('nonMailing.omschrijving') }}</div>
      </div>
      <div class="row">
        <div class="col-md-4">Aantal medewerkers:</div>
        <div class="col-md-8">
          <strong>Fulltime:</strong> {{ get('manifesteertZichAls.onderneming.voltijdWerkzamePersonen') || 0 }}
          | <strong>Parttime:</strong> {{ get('manifesteertZichAls.onderneming.deeltijdWerkzamePersonen') || 0 }}
          | <strong>Totaal:</strong> {{ get('manifesteertZichAls.onderneming.totaalWerkzamePersonen') || 0 }}
        </div>
      </div>
      <div class="row">
        <div class="col-md-4">Aantal vestigingen:</div>
        <div class="col-md-8">{{ getLength('manifesteertZichAls.onderneming.wordtUitgeoefendIn')}}</div>
      </div>

      <h3>Eigenaar</h3>
      <div class="row">
        <div class="col-md-4">Heeft als eigenaar:</div>
        <div class="col-md-8">{{get('heeftAlsEigenaar.item.volledigeNaam')}}</div>
      </div>
      <div class="row">
        <div class="col-md-4"></div>
        <div class="col-md-8">
          <objectTree :item="get('heeftAlsEigenaar')" />
        </div>
      </div>
      <div class="row">
        <div class="col-md-4">Had als eigenaar:</div>
        <div class="col-md-8">{{get('hadAlsEigenaar.item.volledigeNaam')}}</div>
      </div>
      <div class="row">
        <div class="col-md-4"></div>
        <div class="col-md-8">
          <objectTree :item="get('hadAlsEigenaar')" />
        </div>
      </div>

      <template v-if="get('heeftAlsEigenaar.item.bijzondereRechtstoestand')">
        <h3>Bijzondere Rechtstoestand</h3>
        <div class="row">
          <div class="col-md-4">Soort:</div>
          <div class="col-md-8">{{get('heeftAlsEigenaar.item.bijzondereRechtstoestand.soort.omschrijving')}}</div>
        </div>
        <div class="row">
          <div class="col-md-4">Sinds:</div>
          <div class="col-md-8">{{formatDate(get('heeftAlsEigenaar.item.bijzondereRechtstoestand.registratie.registratieTijdstip'))}}</div>
        </div>
        <div class="row">
          <div class="col-md-4">Bij:</div>
          <div class="col-md-8">{{get('heeftAlsEigenaar.item.bijzondereRechtstoestand.item.naam')}}, {{get('heeftAlsEigenaar.item.bijzondereRechtstoestand.item.plaats')}}</div>
        </div>
        <div class="row">
          <div class="col-md-4">Volledig:</div>
          <div class="col-md-8"><objectTree :item="get('heeftAlsEigenaar.item.bijzondereRechtstoestand')" /></div>
        </div>

      </template>

      <h3>Functie Vervullingen</h3>
      <table class="table table-striped table-bordered table-hover" v-if="get('heeftAlsEigenaar.item.heeft')">
        <thead>
          <tr>
            <th>Functie</th>
            <th>FunctieTitel</th>
            <th>VolledigeNaam</th>
            <th>Bevoegdheid</th>
            <th>Volledig</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(val, index) in  get('heeftAlsEigenaar.item.heeft')" :key="index">
            <td>{{get('item.functie.referentieType', val)}}</td>
            <td>{{get('item.functie.omschrijving', val)}}</td>
            <td>{{get('item.item.item.volledigeNaam', val)}}</td>
            <td>{{get('item.bevoegdheid.soort.omschrijving', val)}}</td>
            <td>
              <objectTree :item="val" />
            </td>
          </tr>
        </tbody>
      </table>
      <div v-else>Geen functievervullingen bekend.</div>

      <h3>SBI Activiteiten</h3>
      <table class="table table-striped table-bordered table-hover" v-if="get('manifesteertZichAls.onderneming.sbiActiviteit')">
        <thead>
          <tr>
            <th>SBI Code</th>
            <th>Omschrijving</th>
            <th>HoofdActiviteit?</th>
            <th>Volledig</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(val, index) in get('manifesteertZichAls.onderneming.sbiActiviteit')" :key="index">
            <td>{{get('sbiCode.code', val)}}</td>
            <td>{{get('sbiCode.omschrijving', val)}}</td>
            <td>{{get('isHoofdactiviteit.omschrijving', val)}}</td>
            <td><objectTree :item="val" /></td>
          </tr>
        </tbody>
      </table>
      <div v-else>Geen SBI Activiteiten bekend.</div>

      <h3>Vestigingen</h3>
      <table class="table table-striped table-bordered table-hover vestiging">
        <thead>
          <tr>
            <th>Vestigingsnummer</th>
            <th>Naam</th>
            <th>Telefoon</th>
            <th>Email</th>
            <th>Hoofd?</th>
            <th>Type Adres</th>

            <th>Adres</th>
            <th>Volledig</th>
          </tr>
        </thead>
        <tbody>
          <template v-for="(val, index) in vestigingen">
            <tr :key="index">
              <td rowspan="2">{{get('item.vestigingsnummer', val)}}</td>
              <td rowspan="2">{{get('item.eersteHandelsnaam', val)}}</td>
              <td rowspan="2">
                <span v-for="(gegevens, index) in get('item.communicatiegegevens.communicatienummer', val)" :key="index">
                  {{gegevens.nummer}}
                </span>
              </td>
              <td rowspan="2">
                <span v-for="(gegevens, index) in get('item.communicatiegegevens.emailAdres', val)" :key="index">
                  {{gegevens}}
                </span>
              </td>
              <td rowspan="2">{{get('item.isHoofdVestiging', val) ? 'ja' : 'nee'}}</td>
              <td>Vestiging</td>
              <td>{{get('item.bezoekLocatie.volledigAdres', val)}}</td>
              <td rowspan="2"><objectTree :item="val" /></td>
            </tr>
            <tr :key="`volledigadres-${index}`">
              <td>Postadres</td>
              <td>{{get('item.bezoekLocatie.volledigAdres', val)}}</td>
            </tr>
          </template>
        </tbody>
      </table>


    </div>
  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import moment from 'moment'
  import objectTree from '../utilities/objectTree'

  export default {
    props: ['item'],
    components: { objectTree },

    data() {
      return { showChildren: false }
    },

    computed: {
      kvkItem() {
        return this.item ? this.item.product.maatschappelijkeActiviteit : {};
      },
      vestigingen() {
        const hoofdVestiging = this.get('wordtGeleidVanuit');
        let nietCommercieleVestigingen = this.get('wordtUitgeoefendIn');
        let commercieleVestigingen = this.get('manifesteertZichAls.onderneming.wordtUitgeoefendIn');

        let vestigingen = [];
        if (hoofdVestiging && hoofdVestiging.item) {
          hoofdVestiging.item.isHoofdVestiging = true;
          vestigingen.push(hoofdVestiging);
        }
        
        if (nietCommercieleVestigingen && nietCommercieleVestigingen.length > 0) {
          vestigingen = vestigingen.concat(nietCommercieleVestigingen.map(vestiging => {
            return {
              isHoofdVestiging: false,
              item: vestiging.nietCommercieleVestiging
            };
          }).filter(v => v.item.vestigingsnummer !== hoofdVestiging.item.vestigingsnummer));
        }

        if (commercieleVestigingen && commercieleVestigingen.length > 0) {
          vestigingen = vestigingen.concat(commercieleVestigingen.map(vestiging => {
            return {
              isHoofdVestiging: false,
              item: vestiging.commercieleVestiging
            };
          }).filter(v => v.item.vestigingsnummer !== hoofdVestiging.item.vestigingsnummer));
        }

        return vestigingen;
      },
      success() {
        return this.item
          && this.item.meldingen.informatie
          && (this.item.meldingen.informatie[0].code === 'IPD0000');
      },
    },

    methods: {
      formatDate(date, inputFormat, outputFormat) {
        if (date === '00000000') {
          return '00000000'
        }

        return date ? moment(date, inputFormat || 'YYYYMMDDHHmmssSSS').format(outputFormat || 'YYYY-MM-DD HH:mm:ss') : ''
      },
      get(prop, item) {
        var props = prop.split('.');
        var val = item || this.kvkItem;
        for (let p in props) {
          val = val && props[p] ? val[props[p]] : null;
        }
        return val;
      },
      getLength(prop, item) {
        let array = this.get(prop, item);
        return array ? array.length : 0
      }
    }
  }
</script>

<style scoped>
  h3 {
    margin: 25px 0 10px;
  }
</style>
