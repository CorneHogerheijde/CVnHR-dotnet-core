<template>
  <div>
    <icon v-if="loading" icon="spinner" pulse />
    <template v-if="currentKvkSearch.result">
      <ul class="nav nav-tabs" id="result-tabs" role="tablist">
        <li class="nav-item">
          <a class="nav-link active" id="result-tab" data-toggle="tab" href="#result" role="tab" aria-controls="result" aria-selected="true">
            Overzicht
          </a>
        </li>
        <li class="nav-item">
          <a class="nav-link" id="treeview-tab" data-toggle="tab" href="#treeview" role="tab" aria-controls="treeview" aria-selected="false">
            TreeView
          </a>
        </li>
      </ul>
      <div class="tab-content">
        <div class="tab-pane fade show active" id="result" role="tabpanel" aria-labelledby="result-tab">
          (TODO, formatten!!!)
          {{currentKvkSearch.result}}
        </div>
        <div class="tab-pane fade" id="treeview" role="tabpanel" aria-labelledby="treeview-tab">
          <br />
          <button @click="toggleShowChildren">{{ showAll ? 'alles inklappen' : 'alles uitklappen'}}</button>
          <button @click="toggleShowEmpty">{{ showEmpty ? 'lege velden verbergen' : 'lege velden tonen'}}</button>
          <br /><br />
          <objectTree :item="currentKvkSearch.result"></objectTree>
        </div>
      </div>
    </template>
  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import objectTree from '../utilities/objectTree'

  export default {
    components: { objectTree },

    computed: {
      ...mapState({
        currentKvkSearch: state => state.kvkSearch,
        loading: state => state.kvkSearch.loading
      }),
      showAll() {
        return this.currentKvkSearch.viewSettings && this.currentKvkSearch.viewSettings.showChildren
      },
      showEmpty() {
        return this.currentKvkSearch.viewSettings && this.currentKvkSearch.viewSettings.showEmpty
      },
      arrayOfItems() {
        return this.currentKvkSearch.result
          ? this.toArray(this.currentKvkSearch.result)
          : null;
      }
    },

    methods: {
      ...mapActions(['searchKvk', 'resetKvkSearch', 'updateKvkSearchViewSettings']),
      search: function () {
        if (!!this.currentKvkSearch.kvkNumber) {
          this.$router.push({ query: { kvk: this.currentKvkSearch.kvkNumber } })
        }
      },
      toggleShowChildren: function () {
        this.updateKvkSearchViewSettings({ showChildren: !this.showAll, collapseAll: !this.showAll })
      },
      toggleShowEmpty: function () {
        this.updateKvkSearchViewSettings({ showEmpty: !this.showEmpty })
      }
    },

    watch: {
      '$route'(to, from) {
        if (!!to.query.kvk) {
          this.searchKvk(to.query.kvk);
        } else {
          this.resetKvkSearch()
        }
      }
    },

    created() {
      this.currentKvkSearch.kvkNumber = this.$route.query.kvk || this.currentKvkSearch.kvkNumber
      
      if (!!this.currentKvkSearch.kvkNumber) {
        if (!this.$route.query.kvk) {
          this.search();
        } else {
          this.searchKvk(this.currentKvkSearch.kvkNumber); // TODO, optimize!
        }
      }
    },
  }
</script>

<style scoped>
  a.nav-link {
    color: #212529;
  }
</style>
