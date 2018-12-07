<template>
  <div>
    <div v-if="item && !success">
      TODO, show {{ item.meldingen.informatie }}
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
        <div class="offset-md-4 col-md-12" v-for="h in get('manifesteertZichAls.onderneming.handeltOnder')">
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
        <div class="col-md-8">
          <objectTree :item="get('heeftAlsEigenaar.item')" />
        </div>
      </div>
      <div class="row">
        <div class="col-md-4">Had als eigenaar:</div>
        <div class="col-md-8">
          <objectTree :item="get('hadAlsEigenaar.item')" />
        </div>
      </div>

      <h3>Functie Vervullingen</h3>
      <div class="row" v-for="(val, index) in get('heeftAlsEigenaar.item.heeft')">
        <div class="col-md-4">{{get('item.functie.omschrijving', val)}}:</div>
        <div class="col-md-8">
          <objectTree :item="val.item" />
        </div>
      </div>

      TODO: show rest!

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
      success() {
        return this.item && (this.item.meldingen.informatie[0].code === 'IPD0000');
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
