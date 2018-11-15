import Vue from 'vue'
import Vuex from 'vuex'
import { settingsActions, settingsMutations } from './CVnHRSettings'
import { kvkApiSearchActions, kvkApiSearchMutations } from './kvkApiSearch'
import { kvkSearchActions, kvkSearchMutations } from './kvkSearch'

Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
  counter: 1,
  settings: null,
  kvkApiSearch: { q: null, startPage: 1, result: null, loading: false },
  kvkSearch: { kvkNumber: null, result: null, loading: false }
}

// MUTATIONS
const mutations = {
  ...settingsMutations,
  ...kvkApiSearchMutations,
  ...kvkSearchMutations,
  [MAIN_SET_COUNTER] (state, obj) {
    state.counter = obj.counter
  }
}

// ACTIONS
const actions = ({
  ...settingsActions,
  ...kvkApiSearchActions,
  ...kvkSearchActions,
  setCounter ({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj)
  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions
})
