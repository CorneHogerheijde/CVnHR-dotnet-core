import axios from 'axios'

const searchKvkType = 'searchKvkType'
const resetKvkSearchType = 'resetKvkSearchType'
const updateKvkSearchViewSettingsType = 'updateKvkSearchViewSettingsType'

const kvkSearchActions = {
  searchKvk: async ({ commit }, kvkNumber) => {
    commit(searchKvkType, { kvkNumber })
  },
  resetKvkSearch: async ({ commit }) => {
    commit(resetKvkSearchType)
  },
  updateKvkSearchViewSettings: async ({ commit }, viewSettings) => {
    commit(updateKvkSearchViewSettingsType, viewSettings)
  }
}

const kvkSearchMutations = {
  async [searchKvkType] (state, { kvkNumber }) {
    state.kvkSearch.kvkNumber = kvkNumber
    state.kvkSearch.loading = true

    const url = `api/kvk/${kvkNumber}`

    let result = null
    try {
      let response = await axios.get(url)
      result = response.data
    } catch (err) {
      //window.alert(err)
      console.log(err)
    }
    state.kvkSearch.result = result
    state.kvkSearch.loading = false
  },
  [resetKvkSearchType] (state) {
    state.kvkSearch.result = null
  },
  [updateKvkSearchViewSettingsType] (state, viewSettings) {
    state.kvkSearch.viewSettings = {
      ...state.kvkSearch.viewSettings,
      ...viewSettings
    }
  }
}

export { kvkSearchActions, kvkSearchMutations }
