<template>
  <div>
    <icon v-if="loading" icon="spinner" pulse />

    <p v-if="result && result.totalItems == 0" class="text-danger">Nothing found.</p>
    <div v-if="result && result.totalItems > 0" class="kvk-api-result-table container">
      <p> {{result.totalItems}} items found.</p>

      <div class="row" v-for="item in result.items" v-if="result.totalItems > 0">
        <div class="col-md">{{item.kvkNumber}}</div>
        <div class="col-md-5">{{item.tradeNames === null ? '[??]' : (item.tradeNames.businessName || item.tradeNames.shortBusinessName)}}</div>
        <div class="col-md-4">{{getAddress(item)}}</div>
        <div class="col-md">
          <router-link :to="`/?kvk=${item.kvkNumber}`">zoek</router-link>
        </div>
      </div>

      <br/>
      <div class="controls row justify-content-between">
        <div class="col-md font-italic">
          {{result.totalItems}} resultaten gevonden. Pagina {{result.startPage}} van {{endPage}}
        </div>
        <div class="col-md text-md-right">
          <a href="#" v-if="result.startPage > 1" v-on:click="prevPage">vorige pagina</a>
          <span v-if="result.startPage > 1 && result.startPage < endPage">|</span>
          <a href="#" v-if="result.startPage < endPage" v-on:click="nextPage">volgende pagina</a>
        </div>
      </div>

    </div>

  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'

  export default {
    computed: {
      ...mapState({
        loading: state => state.kvkApiSearch.loading,
        result: state => state.kvkApiSearch.result,
      }),
      endPage: function () {
        const r = this.result
        return r
          ? parseInt(r.totalItems / r.itemsPerPage) + (r.totalItems % r.itemsPerPage > 0 ? 1 : 0)
          : 0;
      }
    },

    methods: {
      nextPage: function (e) {
        e.preventDefault()
        this.$router.push({
          query: { ...this.$route.query, startPage: parseInt(this.$route.query.startPage || 1) + 1 }
        })
        return false
      },
      prevPage: function (e) {
        e.preventDefault()
        this.$router.push({
          query: { ...this.$route.query, startPage: parseInt(this.$route.query.startPage || 1) - 1 }
        })
        return false
      },
      getAddress: function (item) {
        const address = item.addresses ? item.addresses[0] : null
        return address
          ? `${address.street} ${address.houseNumber}${address.houseNumberAddition} ${address.city}`
          : null
      }
    }
  }
</script>

<style scoped>
  a {
    color: blue;
    cursor: pointer;
  }
</style>
