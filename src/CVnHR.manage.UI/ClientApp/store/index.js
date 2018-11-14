import Vue from 'vue'
import Vuex from 'vuex'
import { settingsActions, settingsMutations } from './CVnHRSettings'
import { kvkApiSearchActions, kvkApiSearchMutations } from './kvkApiSearch'

Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
  counter: 1,
  settings: null,
  kvkApiSearch: { q: null }
}

// MUTATIONS
const mutations = {
  ...settingsMutations,
  ...kvkApiSearchMutations,
  [MAIN_SET_COUNTER] (state, obj) {
    state.counter = obj.counter
  }
}

// ACTIONS
const actions = ({
  ...settingsActions,
  ...kvkApiSearchActions,
  setCounter ({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj)
  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions
})
