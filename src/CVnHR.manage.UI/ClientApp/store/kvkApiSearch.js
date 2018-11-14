import axios from 'axios'

const searchKvkApiType = 'searchKvkApiType'

const kvkApiSearchActions = {
  searchKvkApi: async ({ commit }, q, startPage) => {
    commit(searchKvkApiType, q, startPage)
  }
}

const kvkApiSearchMutations = {
  async [searchKvkApiType] (state, q, startPage) {
    console.log('searching q=', q)
    console.log('searching startpage=', startPage)

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
  }
}

export { kvkApiSearchActions, kvkApiSearchMutations }
