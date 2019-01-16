# CVnHR dotnet core


First attempt to rewrite to Dotnet core of this repo: https://github.com/B3Partners/CVnHR-Open-Source

TODO: add more documentation!

## Run the project:

* In Visual studio, set CVnHR.manage.UI as Startup project and hit F5
* Command line: 
  * from location CVnHR.manage.UI run `dotnet run`
  * from root: run `dotnet run --project src/CVnHR.manage.UI/CVnHR.manage.UI.csproj`

<i>take care: probably need to run as admin</i>

## CI/CD

Azure DevOps is used for a basic pipeline, currently the project is build only.
TODO: add tests, both FE and BE unittests and Integration tests (cypress?).

[![Build Status](https://qnh-ontwikkeldesk.visualstudio.com/CVnHR/_apis/build/status/CorneHogerheijde.CVnHR-dotnet-core)](https://qnh-ontwikkeldesk.visualstudio.com/CVnHR/_build/latest?definitionId=3)


## Git conventions
* We use Conventional Changelog, a commit standard pioneered by the Angular project which infers a structure in commit messages. Metadata in these commit messages are used to infer the version number for new releases and to generate a changelog. You can manually type the commit message in the format explained by the linked document.
* Naming convention of development branches:
  * Feature: feature/#[number-of-issue]-name-of-userstory
  * Bugfix: bugfix/#[number-of-issue]-name-of-bug
  * Chore: chore/#[number-of-issue]-name-of-chore
  * Refactor: refactor/#[number-of-issue]-name-of-refactor
  * Style: style/#[number-of-issue]-name-of-style (formatting, missing semi colons, …)
  * Test: test/#[number-of-issue]-name-of-test (when adding missing tests)
  * Docs: docs/#[number-of-issue]-name-of-docs (documentation)

## Git workflow
The master branch is the main branch where all production ready code lives. Changes cannot be done directly on master, but per userstory a development branch can be created. We aim to have only one commit per branch having all changes to add the feature to keep master as clean as possible.

In order to have one commit per branch you can squash all commits on a branch to a single commit.

**Recommendations:**

* Stay as close as possible to one commit on a branch, so squash as soon as you have two commits.
* Try not to work with more than one person on a branch
* Try to keep your branch as close as possible to the commits on master by rebasing master whenever possible

**Rebase actions:**

* <i>Keep your branch up-to-date with master:</i>
    1. ensure you have committed your changes on your branch
    2. checkout master and pull all changes
    3. checkout your branch
    4. open git terminal and run command
    * `git rebase master`
* <i>Squash your commits</i>
  1. open git terminal (third icon from right top in sourcetree) and run the following commands:
    * `git rebase -i HEAD~#` => where # is the number of commits you want to squash
    * `i` => a VIM editor is opened automatically, the `i` command enters edit mode
    * use cursors and keyboard to select the commits to squash => typically you keep the top commit (pick) and change `pick` to `s` for all other commits
    * `esc` => close edit mode in VIM
    * `:wq` => write & quit VIM (mind the colon!)
    * a new VIM editor opens to choose the final commit message
    * `i` + comment out (#) all unwanted commit messages + `esc` + `:wq` => here you choose the final commit message. You can rewrite the commit message completely if needed. (Mind the Conventional Changelog!)
  2. force push your changes. Terminal commands:
    * `git push -f`

# Git Documentation
* https://git-scm.com/doc