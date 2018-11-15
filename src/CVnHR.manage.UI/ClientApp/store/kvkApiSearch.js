import axios from 'axios'

const searchKvkApiType = 'searchKvkApiType'
const resetKvkApiSearchType = 'resetKvkApiSearchType'

const kvkApiSearchActions = {
  searchKvkApi: async ({ commit }, { q, startPage }) => {
    commit(searchKvkApiType, { q, startPage })
  },
  resetKvkApiSearch: async ({ commit }) => {
    commit(resetKvkApiSearchType)
  }
}

const kvkApiSearchMutations = {
  async [searchKvkApiType] (state, { q, startPage }) {
    state.kvkApiSearch.q = q
    state.kvkApiSearch.loading = true

    const url = `api/kvk?q=${q}&startPage=${startPage || 1}`

    let result = null
    try {
      let response = await axios.get(url)
      result = response.data
    } catch (err) {
      window.alert(err)
      console.log(err)
    }
    state.kvkApiSearch.result = result
    state.kvkApiSearch.loading = false
  },
  [resetKvkApiSearchType](state) {
    state.kvkApiSearch.result = null
  }
}

export { kvkApiSearchActions, kvkApiSearchMutations }
